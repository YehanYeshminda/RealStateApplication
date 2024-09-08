import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfoBank } from '../../shared/models/models';
import { LeadforwardHttpService } from '../services/leadforward-http.service';
import { checkForAccess, extractDate, formatDate } from '../../shared/methods';
import { LeadForwardViewResponse, LeadRequest } from '../models/leadforward';
import { GetAuthDetails, formatDateReset } from 'src/app/shared/models/methods';
import { errorNotification, successNotification } from '../../shared/notifications/notification';
import { Router } from '@angular/router';
import { CommonHttpService } from '../../services/common-http.service';

@Component({
  selector: 'app-lead-forward-add-edit',
  templateUrl: './lead-forward-add-edit.component.html',
  styleUrls: ['./lead-forward-add-edit.component.scss']
})
export class LeadForwardAddEditComponent implements OnInit {
  leadsForwardFormControl!: FormGroup;
  submitted = false;
  isEditMode = false;
  isAddAllowed$: Observable<boolean> = of();
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  leadNos$: Observable<ComboInfoBank[]> = of([]);
  editLeadForwardInfo: LeadForwardViewResponse | null = null;

  constructor(private leadForwardHttpService: LeadforwardHttpService, private fb: FormBuilder, private router: Router, private commonHttpService: CommonHttpService) { }

  get form() { return this.leadsForwardFormControl.controls; }

  ngOnInit(): void {
    this.editLeadForwardInfo = history.state;

    this.loadStaff();
    this.loadLeadNos();

    if (this.editLeadForwardInfo?.Id) {
      if (this.editLeadForwardInfo != null) {
        this.initializeForm(this.editLeadForwardInfo);
        this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Edit", "LeadForward");
      }
    } else {
      this.initializeForm();
      this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Add", "LeadForward");
    }
  }

  onSubmit() {
    this.submitted = true;
    const data: LeadRequest = {
      authDto: GetAuthDetails(),
      ...this.leadsForwardFormControl.value
    }

    if (this.isEditMode) {
      this.leadForwardHttpService.editExistingLeadForward(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.router.navigateByUrl("/dashboard/leadForward")
          } else {
            errorNotification(response.message);
          }
        }
      })
    } else {
      this.leadForwardHttpService.addNewLeadForward(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.router.navigateByUrl("/dashboard/leadForward")
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  }

  initializeForm(data?: LeadForwardViewResponse) {
    if (data != null) {
      this.isEditMode = true;
      this.leadsForwardFormControl = this.fb.group({
        id: [data.Id, [Validators.required]],
        date: [formatDateReset(new Date(data.Date)), [Validators.required]],
        leadid: [data.LeadId, [Validators.required]],
        leadName: ['', [Validators.required]],
        forwardstaffid: [data.ForwardTo, []],
        reason: [data.Reason, []],
        leadLog: ['', []]
      })

      this.leadForwardHttpService.getLeadForwardById(data.LeadId).subscribe({
        next: response => {
          this.leadsForwardFormControl.patchValue({
            date: formatDateReset(new Date(response.result.date)),
            leadName: response.result.name,
            leadLog: response.result.log,
          })
        }
      })

      this.staffNos$.subscribe({
        next: response => {
          const index = response.findIndex(x => x.textValue == data.ForwardTo);

          this.leadsForwardFormControl.patchValue({
            forwardstaffid: response[index].value
          })
        }
      })

    } else {
      this.leadsForwardFormControl = this.fb.group({
        date: ['', [Validators.required]],
        leadid: ['', [Validators.required]],
        //leadName: ['', [Validators.required]],
        forwardstaffid: ['', [Validators.required]],
        leadLog: ['', [Validators.required]],
        reason: ['']
      });
  
      this.leadsForwardFormControl.controls['leadid'].valueChanges.subscribe({
        next: leadId => {
          if (leadId === '') return;
          this.leadForwardHttpService.getLeadForwardById(leadId).subscribe({
            next: response => {
              console.log(extractDate(response.result.date))
              this.leadsForwardFormControl.patchValue({
                date: formatDate(new Date(response.result.date)),
                //leadName: response.result.name,
                leadLog: response.result.log,
              })
            }
          })
        }
      })
    }
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    )
  }

  loadLeadNos() {
    this.leadNos$ = this.leadForwardHttpService.getLeadNoAndIdComboDataAll().pipe(
      shareReplay(1)
    );
  }
}
