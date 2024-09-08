import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription, of, retry, shareReplay, tap, timeout } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { Bulkassign, CallInsightRequest, CallListViewData } from '../models/callinsignt';
import { ComboInfoBank } from '../../shared/models/models';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CallCenterHttpService } from '../services/call-center-http.service';
import { LeadforwardHttpService } from '../../lead-forward/services/leadforward-http.service';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { errorNotification, infoNotification, successNotification } from '../../shared/notifications/notification';

@Component({
  selector: 'app-call-segregation',
  templateUrl: './call-segregation.component.html',
  styleUrls: ['./call-segregation.component.scss']
})
export class CallSegregationComponent implements OnInit, OnDestroy {
  callInsignt$: Observable<Root<CallListViewData>> = of();
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  form: FormGroup = new FormGroup({});
  callInsigntIds: number[] = [];
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;

  loadCallsSubscription: Subscription | undefined;
  assignCallInsightsSubscription: Subscription | undefined;
  
  constructor(private callInsightHttpService: CallCenterHttpService, private leadForwardHttpService: LeadforwardHttpService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.loadCalls(this.page);
    this.loadStaff();
    this.initializeForm();
  }

  ngOnDestroy(): void {
    if (this.loadCallsSubscription) {
      this.loadCallsSubscription.unsubscribe();
    }
  }

  tableRefresh() {
    this.loadCalls(this.page);
    this.isPaginating = true;
  }

  loadCalls(page: number) {
    this.loadCallsSubscription = this.callInsightHttpService.getAllCallInsignts(page, this.pageSize).pipe(
      timeout(5000),
      retry(3),
      tap(response => {
        if (response.isSuccess) {
          this.callInsignt$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }),
    ).subscribe();
  }

  onSubmit() {
    if (this.form.controls['assignStaff'].value != "") {
      const data: CallInsightRequest = {
        assignStaff: this.form.controls['assignStaff'].value,
        callInsigntIds: this.callInsigntIds,
        authDto: GetAuthDetails()
      }
  
      this.callInsightHttpService.assignCallInsights(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            if (this.form.controls['counts'].value != '') {
              // successNotification(response.message);
            }
            this.form.reset();
            this.loadCalls(this.page);
          } else {
            errorNotification(response.message);
          }
        }
      })

      if (this.form.controls['counts'].value != '' && this.form.controls['assignStaff'].value != "") {
        this.oncount();
      }
      this.form.reset();
      this.loadCalls(this.page);
    } else {
      infoNotification("Select a staff to assign")
    }
  }

  initializeForm() {
    this.form = this.fb.group({
      assignStaff: ['', [Validators.required]],
      counts : ['', [Validators.required]],
    })
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadCalls(newPage);
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      timeout(5000),
      retry(3),
      shareReplay(1)
    )
  }

  oncount() {
    const numberToAssign = this.form.get('counts')?.value; 
    const data: Bulkassign = {
      assignStaff: this.form.controls['assignStaff'].value,
      authDto: GetAuthDetails(),
      numberOfItemsToAssign: numberToAssign,
    };
  
    this.callInsightHttpService.assignbulk(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    });
  }

  changeStatus(id: number) {
    const index = this.callInsigntIds.indexOf(id);

    if (index === -1) {
      this.callInsigntIds.push(id);
    } else {
      this.callInsigntIds.splice(index, 1); 
    }
  }
}
