import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { Observable, Subscription, catchError, debounceTime, of, retry, shareReplay, switchMap, tap, timeout } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { DeleteLeadRequest, LeadFilterRequest, LeadVList, LeadsViewData, LogRequest, UpdateLeadStatusRequest, leadlogrequest } from './models/list';
import { LeadsHttpService } from './service/leads-http.service';
import { Router } from '@angular/router';
import { CommonHttpService } from '../services/common-http.service';
import { ComboInfo, ComboInfoBank } from '../shared/models/models';
import { confirmApproveNotification, errorNotification, successNotification } from '../shared/notifications/notification';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LeadforwardHttpService } from '../lead-forward/services/leadforward-http.service';
import { UnsubscribeHelper } from 'src/app/shared/helpers';
import { DEBOUNCE_1000, DEBOUNCE_2000, ERROR_MESSAGE, FIVE_THOUSAND_TIMEOUT_TIME, RETRY_COUNT } from 'src/app/shared/times';

@Component({
  selector: 'app-leads',
  templateUrl: './leads.component.html',
  styleUrls: ['./leads.component.scss']
})

export class LeadsComponent implements OnInit, OnDestroy {
  leads$: Observable<Root<LeadsViewData>> = new Observable();
  totalData!: number;
  page = 1;
  pageSize = 20;
  isPaginating: boolean = false;
  leadStatus$: ComboInfo[] = [];
  isDropDownClicked: boolean = false;
  index: number = 0;
  value: string = '';
  leadGroup: FormGroup = new FormGroup({});
  selectedLeadStatus = "";
  selectedLead: any;
  selectedLeadNo!: string;
  selectedLeadLog: string[] = [];
  logText: string = '';
  staffNos$: Observable<ComboInfoBank[]> = of([]);

  getLeadLog: string[] = [];

  private leadSubscription: Subscription | undefined;
  private leadFilterUsingStatusSubscription: Subscription | undefined;
  private leadDeleteSubscription: Subscription | undefined;
  private leadStatusUpdateSubscription: Subscription | undefined;
  private leadLoadStatusSubscription: Subscription | undefined;
  private leadLogAllSubscription: Subscription | undefined;
  private leadLogAddSubscription: Subscription | undefined;
  public leadFilterByNameStaffImportanceSubscription: Subscription | undefined;
  private leadStatusValueChangeSubscription: Subscription | undefined;
  private leadStatusStaffValueChangeSubscription: Subscription | undefined;
 
  ifFiltered = false;

  constructor(
    private leadsHttpService: LeadsHttpService, 
    private router: Router, 
    private commonHttpService: CommonHttpService, 
    private fb :FormBuilder,
    private modalService: NgbModal,
    private leadForwardHttpService: LeadforwardHttpService
    ) { }

  isClicked(index: number) {
    this.isDropDownClicked = true;
    this.index = index;
  }

  initializeForm() {
    this.leadGroup = this.fb.group({
      leadStatus: ['', []],
      staffId: ['', []]
    });

    this.leadStatusValueChangeSubscription =this.leadGroup.controls['leadStatus'].valueChanges.subscribe({
      next: response => {
        this.selectedLeadStatus = response;
        this.loadLeadsLeadsDependingOnStatus(this.selectedLeadStatus, 1);
      }
    });

    this.leadStatusStaffValueChangeSubscription = this.leadGroup.controls['staffId'].valueChanges.subscribe({
      next: response => {
        if (response) {
          const leadStatus = this.leadGroup.controls['leadStatus'].value;

          if (leadStatus == "") {
            this.leadGroup.controls['leadStatus'].setValue("21");
            this.filterLeadByStaffAndImportance(1, response, "21");
          } else {
            this.filterLeadByStaffAndImportance(1, response, leadStatus);
          }
        }
      }
    })
  }

  /**
   * Filters leads by staff and importance.
   * @param page - The page number to retrieve.
   * @param staffId - The ID of the staff to filter by.
   * @param importance - The importance level to filter by.
   */
  filterLeadByStaffAndImportance(page: number, staffId: number , importance: string) {
    this.leadFilterByNameStaffImportanceSubscription = this.leadsHttpService.filterLeadStatusAndStaff(staffId, importance, page).pipe(
        timeout(FIVE_THOUSAND_TIMEOUT_TIME),
        retry(RETRY_COUNT),
        debounceTime(DEBOUNCE_1000),
        switchMap(response => {
          if (response.isSuccess) {
            this.leads$ = of(response);
            this.totalData = response.result.totalData;
            this.isPaginating = false;
            this.ifFiltered = true;
          }

          return of([]);
        }),
      )
      .subscribe();
  }

  /**
   * Loads leads data from the server.
   * @param page The page number to load.
   */
  loadLeads(page: number): void {
    this.leadSubscription = this.leadsHttpService.getAllLeads(page, this.pageSize).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(3),
      debounceTime(DEBOUNCE_1000),
      switchMap(response => {
        if (response.isSuccess) {
          this.totalData = response.result.totalData;
          this.isPaginating = false;
          this.leads$ = of(response);
        }

        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }
  
  /**
   * Loads the staff combo ID and stores it in the `staffNos$` observable.
   */
  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    );
  }

  openmodel(content: TemplateRef<NgbModal>, leadNo: string): void {
    this.selectedLeadNo = leadNo;
    this.loadLeadlogs(this.selectedLeadNo, content);
  }
  
  /**
   * Loads lead logs for a given lead number and opens a modal to display the logs.
   * @param lead - The lead number to load logs for.
   * @param content - The template reference for the modal content.
   * @returns void
   */
  loadLeadlogs(lead: string, content: TemplateRef<NgbModal>): void {
    const data: leadlogrequest = {
      leadNo: lead,
      authDto: GetAuthDetails(),
    };

    this.leadLogAllSubscription = this.leadsHttpService.getAllLeadLogs(data).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_1000),
      switchMap(response => {
        if (response.isSuccess) {
          if (Array.isArray(response.result) && response.result.length > 0) {
            this.selectedLeadLog = response.result;
          } else {
            this.selectedLeadLog = ['No log data available'];
          }

          this.modalService.open(content, { backdrop: 'static', keyboard: false });
        }

        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }
  
  /**
   * Adds a new log for the selected lead.
   * @param content - The template reference for the modal content.
   */
  addlog(content: TemplateRef<NgbModal>) {
    const data: LogRequest = {
      authDto: GetAuthDetails(),
      leadNo: this.selectedLeadNo, 
      log: this.logText 
    }
  
    this.leadLogAddSubscription = this.leadsHttpService.addNewlog(data).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_1000),
      switchMap(response => {
        if (response.isSuccess) {
          successNotification("Successfully added log");
          this.modalService.dismissAll();
          this.logText = '';
        } else {
          errorNotification(response.message);
        }

        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }
  

  closethis(content: TemplateRef<NgbModal>): void {
    this.modalService.dismissAll(content);
  }

  onPageChange(newPage: number): void {
    if (this.ifFiltered) {
      this.page = newPage;
      this.isPaginating = true;
      this.loadLeadsLeadsDependingOnStatus(this.selectedLeadStatus, newPage);
    } else {
      this.page = newPage;
      this.isPaginating = true;
      this.loadLeads(newPage);
    }
  }

  editLead(data: LeadVList) {
    this.router.navigateByUrl('/dashboard/leadadd', { state: data });
  }

  loadLeadStatus() {
    const query = "SELECT id as _Id, leadstatus as _Value FROM tblLeadStatus WHERE status = 0 ORDER BY id DESC"

    this.leadLoadStatusSubscription = this.commonHttpService.getComboBoxData(query).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_2000),
      switchMap(response => {
        this.leadStatus$ = response;

        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }

  onSelectChange(event: any, leadNo: string, index: number) {
    const data: UpdateLeadStatusRequest = {
      leadNo: leadNo,
      status: event.target.value,
      authDto: GetAuthDetails()
    }

    this.leadStatusUpdateSubscription = this.leadsHttpService.updateLeadstatus(data).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_2000),
      switchMap(response => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.isDropDownClicked = false;
          this.loadLeads(1);
          this.isPaginating = true;
        };

        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }

  tableRefresh() {
    this.loadLeads(1);
    this.isDropDownClicked = false;
    this.isPaginating = true;
  }

  deleteLead(lead: string) {
    const data: DeleteLeadRequest = {
      leadNo: lead,
      authDto: GetAuthDetails()
    };

    confirmApproveNotification("Are you sure you want to delete this lead?").then(response => {
      if (response.isConfirmed) {
        this.leadDeleteSubscription = this.leadsHttpService.deleteLead(data).pipe(
          timeout(FIVE_THOUSAND_TIMEOUT_TIME),
          retry(RETRY_COUNT),
          debounceTime(DEBOUNCE_2000),
          switchMap(response => {
            if (response.isSuccess) {
              successNotification(response.message);
              this.loadLeads(this.page);
            } else {
              errorNotification(response.message);
            }

            return of([]);
          }),
          catchError(error => {
            errorNotification(ERROR_MESSAGE);
            throw error;
          }),
        ).subscribe();
      }
    })
  }

  loadLeadsLeadsDependingOnStatus(value: any, page: number) {
    const data: LeadFilterRequest = {
      leadStatus: value,
      authDto: GetAuthDetails(),
      page: page,
      pageSize: 10,
    };

    this.leadFilterUsingStatusSubscription = this.leadsHttpService.filterLeadsStatus(data).pipe(
      timeout(FIVE_THOUSAND_TIMEOUT_TIME),
      retry(RETRY_COUNT),
      debounceTime(DEBOUNCE_2000),
      switchMap(response => {
        if (response.isSuccess) {
          this.leads$ = of(response);
          this.totalData = response.result.totalData;
          this.isPaginating = false;
          this.ifFiltered = true;
        }

        return of([]);
      }),
      catchError(error => {
        errorNotification(ERROR_MESSAGE);
        throw error;
      }),
    ).subscribe();
  }

  ngOnInit(): void {
    this.loadLeadStatus();
    this.initializeForm();
    this.loadStaff();
    this.loadLeads(this.page);
  }

  ngOnDestroy(): void {
    UnsubscribeHelper(
      this.leadSubscription, 
      this.leadFilterUsingStatusSubscription, 
      this.leadDeleteSubscription, 
      this.leadStatusUpdateSubscription, 
      this.leadLoadStatusSubscription,
      this.leadLogAllSubscription,
      this.leadLogAddSubscription,
      this.leadFilterByNameStaffImportanceSubscription,
      this.leadStatusValueChangeSubscription,
      this.leadStatusStaffValueChangeSubscription
    );
  }
}
