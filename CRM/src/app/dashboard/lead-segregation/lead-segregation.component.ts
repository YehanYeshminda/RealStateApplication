import { Component, OnInit } from '@angular/core';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { LeadsforwardService } from '../leadsforward/Service/leadsforward.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LeadSegregationRequest, LeadStaffResponse } from './models/lead-segragetion';
import { checkForAccess, getAuthDetails } from 'src/app/shared/methods/method';
import { LeadSegregationHttpService } from './service/lead-segregation-http.service';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { Root } from 'src/app/shared/models/baseResponse';
import { CommonHttpService } from '../common/common-http.service';

@Component({
  selector: 'app-lead-segregation',
  templateUrl: './lead-segregation.component.html',
  styleUrls: ['./lead-segregation.component.scss']
})
export class LeadSegregationComponent implements OnInit {
  leadNo$: Observable<ComboInfoBank[]> = of([]);
  selectedLeadsNos: string[] = [];
  selectedNo: number = 0;
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  form: FormGroup = new FormGroup({});
  leadsByStaff$: Observable<Root<LeadStaffResponse[]>> = of();
  leadMode = false;
  isAddAllowedToAssign$: Observable<boolean> = of();
  isAddAllowedToRemove$: Observable<boolean> = of();

  constructor(
    private leadForwardHttpService: LeadsforwardService,
    private fb: FormBuilder,
    private leadSegregationHttpService: LeadSegregationHttpService,
    private commonHttpService: CommonHttpService
  ) { }

  ngOnInit(): void {
    this.loadLeadNos();
    this.loadStaff();
    this.initializeForm();

    this.isAddAllowedToAssign$ = checkForAccess(this.commonHttpService, "Add", "LeadSegregation");
    this.isAddAllowedToRemove$ = checkForAccess(this.commonHttpService, "Edit", "LeadSegregation");
  }

  initializeForm() {
    this.form = this.fb.group({
      staffid: ['', [Validators.required]],
      remark: ['', [Validators.required]],
    })

    this.form.controls['staffid'].valueChanges.subscribe({
      next: staffId => {
        this.leadMode = true;
        this.leadsByStaff$ = this.leadSegregationHttpService.getAllLeadsByStaffId(staffId);
      }
    })
  }

  loadLeadNos() {
    this.leadNo$ = this.leadForwardHttpService.getLeadNoAndIdComboData().pipe(
      shareReplay(1)
    );
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    );
  }

  selectedLeads(leadNo: string) {
    const index = this.selectedLeadsNos.findIndex(x => x === leadNo);
    if (index !== -1) {
      this.selectedLeadsNos.splice(index, 1);
    } else {
      this.selectedLeadsNos.push(leadNo);
    }

    this.selectedNo = this.selectedLeadsNos.length;
  }

  assignLeads() {
    const data: LeadSegregationRequest = {
      authDto: getAuthDetails(),
      leadid: this.selectedLeadsNos,
      ...this.form.value
    }

    this.leadSegregationHttpService.addNewSegregation(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  removeLeads() {
    const data: LeadSegregationRequest = {
      authDto: getAuthDetails(),
      leadid: this.selectedLeadsNos,
      ...this.form.value
    }

    this.leadSegregationHttpService.removeExistingLeadSegregation(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

}
