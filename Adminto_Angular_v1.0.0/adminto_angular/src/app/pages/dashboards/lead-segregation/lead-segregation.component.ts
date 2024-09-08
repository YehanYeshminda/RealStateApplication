import { Component, OnInit } from '@angular/core';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfoBank } from '../shared/models/models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Root } from 'src/app/shared/models/base';
import { LeadSegregationRequest, LeadStaffResponse } from './models/leadsegregation';
import { LeadforwardHttpService } from '../lead-forward/services/leadforward-http.service';
import { LeadSegreationHttpService } from './services/lead-segreation-http.service';
import { CommonHttpService } from '../services/common-http.service';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { errorNotification, successNotification } from '../shared/notifications/notification';

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
  form!: FormGroup;
  leadsByStaff$: Observable<Root<LeadStaffResponse[]>> = of();
  leadMode = false;
  submitted = false;

  get form1() { return this.form.controls; }

  constructor(
    private leadForwardHttpService: LeadforwardHttpService,
    private fb: FormBuilder,
    private leadSegregationHttpService: LeadSegreationHttpService,
    private commonHttpService: CommonHttpService
    ) { }

  ngOnInit(): void {
    this.loadLeadNos();
    this.loadStaff();
    this.initializeForm();
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
      authDto: GetAuthDetails(),
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
      authDto: GetAuthDetails(),
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
