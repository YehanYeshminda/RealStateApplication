import { Component, OnDestroy, OnInit } from '@angular/core';
import { StaffHttpService } from '../services/staff-http.service';
import { Observable, Subscription, catchError, debounceTime, of, retry, shareReplay, switchMap, timeout } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AssignedCallsPersonInsigntResultAll } from '../services/assignedcallsperson';
import { DEBOUNCE_1000, ERROR_MESSAGE, FIVE_THOUSAND_TIMEOUT_TIME, RETRY_COUNT } from 'src/app/shared/times';
import { errorNotification, successNotification } from '../../shared/notifications/notification';
import { UnsubscribeHelper } from 'src/app/shared/helpers';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LeadforwardHttpService } from '../../lead-forward/services/leadforward-http.service';
import { ComboInfoBank } from '../../shared/models/models';

@Component({
  selector: 'app-remove-assigned-calls',
  templateUrl: './remove-assigned-calls.component.html',
  styleUrls: ['./remove-assigned-calls.component.scss']
})
export class RemoveAssignedCallsComponent implements OnInit, OnDestroy {
  totalData!: number;
  page = 1;
  pageSize = 10;
  isPaginating = false;
  selectedUserId = 0;

  submitted = false;

  get form() { return this.leadGroup.controls; }

  leadGroup: FormGroup = new FormGroup({});

  allStaffCalls$: Observable<Root<AssignedCallsPersonInsigntResultAll>> = of();
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  
  staffCallsSubcription: Subscription | undefined;
  staffCallFilterSubcription: Subscription | undefined;
  removeAssignedCallsSubcription: Subscription | undefined;

  constructor(private fb: FormBuilder, private staffHttpService: StaffHttpService, private leadForwardHttpService: LeadforwardHttpService) { }

  initializeForm() {
    this.leadGroup = this.fb.group({
      staffId: ['23', [Validators.required]],
      count: [1, [Validators.required, Validators.min(1)]],
    });

    this.staffCallFilterSubcription = this.leadGroup.controls['staffId'].valueChanges.subscribe((value) => {
      if (value) {
        this.loadCalls(this.page, value);
      }
    });
  }

  removeCalls() {
    this.submitted = true;

    const count = this.leadGroup.controls['count'].value;
    const staffId = this.leadGroup.controls['staffId'].value;

    if (this.leadGroup.invalid) {
      return;
    }

    this.removeAssignedCallsSubcription = this.staffHttpService.removeAssignedCallsFromUser(staffId, count).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_1000),
      switchMap(response => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.submitted = false;
          this.loadCalls(this.page, staffId);
        } else {
          errorNotification(response.message);
          this.submitted = false;
        }
        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        this.submitted = false;
        throw error;
      }),
    ).subscribe();
  }

  loadCalls(page: number, userId: number) {
    this.staffCallsSubcription = this.staffHttpService.assignedCallsForStaffToday(userId, page, this.pageSize).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_1000),
      switchMap(response => {
        if (response.isSuccess) {
          this.totalData = response.result.totalData;
          this.isPaginating = false;
          this.allStaffCalls$ = of(response);
          this.selectedUserId = userId;
        }
        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }

  tableRefresh() {
    this.loadCalls(this.page, 23);
  }


  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(3),
      shareReplay(1)
    )
  }

  onPageChange(page: number) {
    if (this.selectedUserId == 0) {
      this.selectedUserId = 23;
    }

    this.isPaginating = true;
    this.page = page;
    this.loadCalls(page, this.selectedUserId);
  }

  ngOnInit(): void {
    this.initializeForm();
    this.loadCalls(this.page, 23);
    this.loadStaff();
  }

  ngOnDestroy(): void {
    UnsubscribeHelper(this.staffCallsSubcription, this.staffCallFilterSubcription);
  }
}
