import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LeadForwardResponse, LeadForwardViewResponse, LeadRequest } from './models/leadforward';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { LeadsforwardService } from './Service/leadsforward.service';
import { checkForAccess, formatDate, formatDateReset, getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { CommonHttpService } from '../common/common-http.service';
import { Router } from '@angular/router';
import { dA } from '@fullcalendar/core/internal-common';

@Component({
  selector: 'app-leadsforward',
  templateUrl: './leadsforward.component.html',
  styleUrls: ['./leadsforward.component.scss']
})
export class LeadsforwardComponent implements OnInit {
  isEditMode: boolean = false;
  form: FormGroup = new FormGroup({});
  leadNos$: Observable<ComboInfoBank[]> = of([]);
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  editLeadForwardInfo: LeadForwardViewResponse | null = null;
  isAddAllowed$: Observable<boolean> = of();

  constructor(
    private fb: FormBuilder,
    private leadForwardHttpService: LeadsforwardService,
    private coomonHttpService: CommonHttpService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.editLeadForwardInfo = history.state;
    this.loadLeadNos();
    this.loadLeadForwards();

    if (this.editLeadForwardInfo?.Id) {
      if (this.editLeadForwardInfo != null) {
        this.initializeForm(this.editLeadForwardInfo);
        this.isAddAllowed$ = checkForAccess(this.coomonHttpService, "Edit", "LeadForward");
      }
    } else {
      this.initializeForm();
      this.isAddAllowed$ = checkForAccess(this.coomonHttpService, "Add", "LeadForward");
    }

  }

  onSubmit() {
    const data: LeadRequest = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    if (this.isEditMode) {
      this.leadForwardHttpService.editExistingLeadForward(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.router.navigateByUrl("/dashboard/leadsforwardlist")
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
            this.router.navigateByUrl("/dashboard/leadsforwardlist")
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
      this.form = this.fb.group({
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
          this.form.patchValue({
            date: formatDateReset(new Date(response.result.date)),
            leadName: response.result.name,
            leadLog: response.result.log,
          })
        }
      })

      this.staffNos$.subscribe({
        next: response => {
          const index = response.findIndex(x => x.textValue == data.ForwardTo);

          this.form.patchValue({
            forwardstaffid: response[index].value
          })
        }
      })

    } else {
      this.isEditMode = false;
      this.form = this.fb.group({
        date: ['', [Validators.required]],
        leadid: ['', [Validators.required]],
        leadName: ['', [Validators.required]],
        forwardstaffid: ['', [Validators.required]],
        reason: ['', [Validators.required]],
        leadLog: ['', [Validators.required]]
      })
    }

    this.form.controls['leadid'].valueChanges.subscribe({
      next: leadId => {
        if (leadId === '') return;
        this.leadForwardHttpService.getLeadForwardById(leadId).subscribe({
          next: response => {
            this.form.patchValue({
              date: formatDate(new Date(response.result.date)),
              leadName: response.result.name,
              leadLog: response.result.log,
            })
          }
        })
      }
    })
  }

  loadLeadForwards() {
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
