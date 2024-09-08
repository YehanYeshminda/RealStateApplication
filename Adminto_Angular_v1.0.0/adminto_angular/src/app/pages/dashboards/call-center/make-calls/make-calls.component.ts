import {
  Component,
  OnDestroy,
  OnInit,
  TemplateRef,
} from '@angular/core';
import {
  Observable,
  Subscription,
  catchError,
  of,
  retry,
  shareReplay,
  switchMap,
  tap,
  throttleTime,
  timeout,
} from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AddNewLeadConversion } from '../models/callinsignt';
import { CallCenterHttpService } from '../services/call-center-http.service';
import {
  errorNotification,
  infoNotification,
  successNotification,
} from '../../shared/notifications/notification';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  ComboInfo,
  MakeCallCenterAssign,
  NotinterestedDto,
} from '../../shared/models/models';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { CommonHttpService } from '../../services/common-http.service';
import {
  CallListPaginationAll,
  MakeCallResponse,
  MakeCenterPaginationRequest,
} from './models/ makecall';
import { SendDynamicFormRequest } from '../../staff/models/staff';
import { UnsubscribeHelper } from 'src/app/shared/helpers';
import {
  ERROR_MESSAGE,
  FIVE_THOUSAND_TIMEOUT_TIME,
  FOUR_THOUSAND_TIMEOUT_TIME,
  RETRY_COUNT,
  THOUSAND_THROTTLE_TIME,
  THREE_THOUSAND_THROTTLE_TIME,
  TWO_THOUSAND_THROTTLE_TIME,
} from 'src/app/shared/times';

@Component({
  selector: 'app-make-calls',
  templateUrl: './make-calls.component.html',
  styleUrls: ['./make-calls.component.scss'],
})
export class MakeCallsComponent implements OnInit, OnDestroy {
  callInsignts$: Observable<Root<CallListPaginationAll>> = of();
  tabs1: number = 1;
  form: FormGroup = new FormGroup({});

  totalPages!: number;
  page = 1;
  pageSize = 15;
  isPaginating: boolean = false;
  isTimeOut = true;

  isLeadStatusShow = false;
  isNotInterestedShow = false;

  leadStatus$: Observable<ComboInfo[]> = of([]);
  rfpts$: Observable<ComboInfo[]> = of([]);
  planToDo$: Observable<ComboInfo[]> = of([]);

  loadInformation$: Observable<MakeCallResponse> = of();

  formRsvp: FormGroup = new FormGroup({});
  formLost: FormGroup = new FormGroup({});
  formNotInterested: FormGroup = new FormGroup({});
  formRfptAdd: FormGroup = new FormGroup({});
  formLeadStatusAdd: FormGroup = new FormGroup({});
  formPlanToDoAdd: FormGroup = new FormGroup({});

  selectedPhone: string = '';
  selectedName: string = '';
  selectedEmail: string = '';

  isRsvp: null | boolean = null;
  isLost: null | boolean = null;

  notInterested = false;
  callBack = false;
  dnd = false;
  voicemail = false;
  isInvalidNo = false;

  submitted = false;
  submittedRfptAdd = false;

  isCrossSegment = false;
  isCrossSegmentLost = false;
  isCrossSegmentNotInterested = false;

  isAddingRfpt = false;
  isAddingLost = false;
  isAddingNotInterested = false;

  // subcriptions
  private callEndTimeSubcription!: Subscription;
  private callGetAllSubcription: Subscription | undefined;
  private leadConversionSubcription: Subscription | undefined;
  private RSVPConversionSubcription: Subscription | undefined;
  private loadCallsSubcription: Subscription | undefined;
  private lostConversionSubcription: Subscription | undefined;
  private notInterestedConversionSubcription: Subscription | undefined;

  get form1() {
    return this.form.controls;
  }
  get rsvpFormValidator() {
    return this.formRsvp.controls;
  }
  get IsLostValidator() {
    return this.formLost.controls;
  }
  get rsptAddEditValidator() {
    return this.formRfptAdd.controls;
  }
  get leadStatusAddEditValidator() {
    return this.formRfptAdd.controls;
  }

  constructor(
    private callCenterHttpService: CallCenterHttpService,
    private modalService: NgbModal,
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.intializeRsvp();
    this.intializeLost();
    this.intializeNotInterested();
    this.initializeLeadStatus();
    this.initializePlanToDo();

    this.loadLeadStatus();
    this.loadCallInsignts(this.page);
    this.initializeRfptModal();
    this.loadRfpt();
    this.loadPlanToDo();
  }

  hardRefresh() {
    this.loadCallInsignts(this.page);
  }

  loadCallInsignts(page: number) {
    this.isPaginating = true;
    this.isTimeOut = false;

    const data: MakeCenterPaginationRequest = {
      authDto: GetAuthDetails(),
      page: page,
      pageSize: this.pageSize,
    };

    this.callGetAllSubcription = this.callCenterHttpService
      .getAllCallInsigntsForUser(data)
      .pipe(
        throttleTime(THOUSAND_THROTTLE_TIME),
        timeout(THREE_THOUSAND_THROTTLE_TIME),
        retry(1),
        tap((response) => {
          if (response.isSuccess) {
            this.callInsignts$ = of(response);
            this.totalPages = response.result.totalData;
            this.isPaginating = false;
          }
        }),
        catchError(() => {
          this.isPaginating = false;
          errorNotification(ERROR_MESSAGE);
          return of([]);
        })
      )
      .subscribe();
  }

  loadCallInsigntsAfterRefresh(page: number) {
    this.isPaginating = true;

    const data: MakeCenterPaginationRequest = {
      authDto: GetAuthDetails(),
      page: page,
      pageSize: this.pageSize,
    };

    this.loadCallsSubcription = this.callCenterHttpService
      .getAllCallInsigntsForUser(data)
      .pipe(
        throttleTime(TWO_THOUSAND_THROTTLE_TIME),
        timeout(4000),
        retry(1),
        tap((response) => {
          if (response.isSuccess) {
            this.callInsignts$ = of(response);
            this.totalPages = response.result.totalData;
            this.isPaginating = false;
          }
        }),
        catchError(() => {
          this.isPaginating = false;
          errorNotification(ERROR_MESSAGE);
          return of([]);
        })
      )
      .subscribe();
  }

  isActive(activeStatus: number) {
    if (activeStatus == 1) {
      this.isLeadStatusShow = true;
      this.isNotInterestedShow = false;
      console.log(this.isLeadStatusShow);
      return;
    } else {
      this.isLeadStatusShow = false;
      this.isNotInterestedShow = true;
      return;
    }
  }

  open(
    content: TemplateRef<NgbModal>,
    phone: string,
    callType: string,
    email: string,
    firstName: string,
    lastName: string
  ): void {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
    this.selectedPhone = phone;
    this.selectedEmail = email;
    this.selectedName = firstName + ' ' + lastName;
    this.tabs1 = 1;

    this.formRsvp.controls['attending'].patchValue(' ');
    this.formRsvp.controls['rsvpType'].patchValue(0);
    this.formRsvp.controls['comments'].patchValue(' ');

    this.isRsvp = false;
    this.isLost = false;

    this.formLost.controls['project'].patchValue(' ');
    this.formLost.controls['clientIs'].patchValue(0);
    this.formLost.controls['planToDo'].patchValue(0);
    this.formLost.controls['when'].patchValue(new Date().toISOString());
    this.formLost.controls['comments'].patchValue(' ');

    this.notInterested = false;
    this.callBack = false;
    this.dnd = false;
    this.voicemail = false;
    this.isInvalidNo = false;
    this.formNotInterested.controls['comments'].patchValue(' ');

    if (this.selectedPhone != '') {
      this.form.controls['name'].patchValue(this.selectedName);
      this.form.controls['email'].patchValue(this.selectedEmail);
    }
  }

  modalClose() {
    this.modalService.dismissAll();
  }

  openRfptModal(content: TemplateRef<NgbModal>) {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  openLeadModal(content: TemplateRef<NgbModal>) {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  openPlanToDoModal(content: TemplateRef<NgbModal>) {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  initializeForm() {
    this.form = this.fb.group({
      name: ['', []],
      email: ['', []],
    });
  }

  initializeRfptModal() {
    this.formRfptAdd = this.fb.group({
      catergoryName: ['', []],
      remark: ['', []],
    });
  }

  initializePlanToDo() {
    this.formPlanToDoAdd = this.fb.group({
      catergoryName: ['', []],
      remark: ['', []],
    });
  }

  initializeLeadStatus() {
    this.formLeadStatusAdd = this.fb.group({
      catergoryName: ['', []],
      remark: ['', []],
    });
  }

  intializeRsvp() {
    this.formRsvp = this.fb.group({
      attending: ['', []],
      rsvpType: ['0', []],
      comments: ['', []],
      cross: [false, []],
    });
  }

  intializeLost() {
    this.formLost = this.fb.group({
      project: ['', []],
      clientIs: ['0', []],
      planToDo: ['0', []],
      when: [new Date(), []],
      comments: ['', []],
      cross: [false, []],
    });
  }

  intializeNotInterested() {
    this.formNotInterested = this.fb.group({
      comments: ['', []],
      cross: [false, []],
      status: ['0', []],
    });
  }

  onScheduleViewing() {
    const values: MakeCallCenterAssign = {
      email: this.selectedPhone,
      remark: this.form.controls['remark'].value,
      authDto: GetAuthDetails(),
      status: 'Scheduled Viewing',
    };

    this.callCenterHttpService.addNewConvertionToCallInsignt(values).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.modalService.dismissAll();
          this.saveEndTime();
        } else {
          errorNotification(response.message);
        }
      },
    });

    this.leadConversionSubcription = this.callCenterHttpService
      .addNewConvertionToCallInsignt(values)
      .pipe(
        throttleTime(THOUSAND_THROTTLE_TIME),
        timeout(FIVE_THOUSAND_TIMEOUT_TIME),
        retry(RETRY_COUNT),
        tap((response) => {
          if (response.isSuccess) {
            this.loadCallInsigntsAfterRefresh(this.page);
            this.modalService.dismissAll();
          }
        }),
        catchError(() => {
          errorNotification(ERROR_MESSAGE);
          this.modalService.dismissAll();
          return of([]);
        })
      )
      .subscribe();
  }

  saveEndTime() {
    this.callEndTimeSubcription = this.callCenterHttpService
      .updateCallIsightEndTime(this.selectedPhone)
      .pipe(
        throttleTime(THOUSAND_THROTTLE_TIME),
        timeout(FIVE_THOUSAND_TIMEOUT_TIME),
        retry(RETRY_COUNT),
        tap((response) => {
          if (response.isSuccess) {
            this.loadCallInsigntsAfterRefresh(this.page);
            this.modalService.dismissAll();
          }
        }),
        catchError(() => {
          errorNotification(ERROR_MESSAGE);
          return of([]);
        })
      )
      .subscribe();
  }

  onCallback() {
    const values: MakeCallCenterAssign = {
      email: this.selectedPhone,
      remark: this.form.controls['remark'].value,
      authDto: GetAuthDetails(),
      status: 'Call Back',
    };

    this.callCenterHttpService.addNewConvertionToCallInsignt(values).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.modalService.dismissAll();
          this.saveEndTime();
        } else {
          errorNotification(response.message);
        }
      },
    });
  }

  onDND() {
    const values: NotinterestedDto = {
      email: this.selectedPhone,
      authDto: GetAuthDetails(),
      events: 'DND',
      // remark: this.form1.controls['remark'].value
    };

    this.callCenterHttpService.newDND(values).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.modalService.dismissAll();
          this.saveEndTime();
        } else {
          errorNotification(response.message);
        }
      },
    });
  }

  notanswer() {
    const values: NotinterestedDto = {
      email: this.selectedPhone,
      authDto: GetAuthDetails(),
      events: 'notanswering',
      // remark: this.form1.controls['remark'].value
    };

    this.callCenterHttpService.newDND(values).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.modalService.dismissAll();
          this.saveEndTime();
        } else {
          errorNotification(response.message);
        }
      },
    });
  }

  addNewDesignation() {
    const values: SendDynamicFormRequest = {
      authDto: GetAuthDetails(),
      dynamicField: 'RFPT',
      ...this.formRfptAdd.value,
    };

    this.commonHttpService.addDynamicData(values).subscribe({
      next: (response) => {
        if (response) {
          successNotification('Success while adding new RSPT');
          this.modalService.dismissAll();
          this.loadRfpt();
        }
      },
      error: (response) => {
        errorNotification('Error while adding new RSPT');
      },
    });
  }

  addNewLeadStatus() {
    const values: SendDynamicFormRequest = {
      authDto: GetAuthDetails(),
      dynamicField: 'LeadStatus',
      ...this.formLeadStatusAdd.value,
    };

    this.commonHttpService.addDynamicData(values).subscribe({
      next: (response) => {
        if (response) {
          successNotification('Success while adding new Lead Status');
          this.modalService.dismissAll();
          this.loadLeadStatus();
        }
      },
      error: (response) => {
        errorNotification('Error while adding new lead status');
      },
    });
  }

  addNewPlanToDo() {
    const values: SendDynamicFormRequest = {
      authDto: GetAuthDetails(),
      dynamicField: 'PlanToDos',
      ...this.formPlanToDoAdd.value,
    };

    this.commonHttpService.addDynamicData(values).subscribe({
      next: (response) => {
        if (response) {
          successNotification('Success while adding new plan to do');
          this.modalService.dismissAll();
          this.loadPlanToDo();
        }
      },
      error: (response) => {
        errorNotification('Error while adding new plan to do');
      },
    });
  }

  statusChange(event: any, statusType: string) {
    if (statusType == 'RSVP') {
      this.isRsvp = true;
      this.isLost = false;
    } else {
      this.isLost = true;
      this.isRsvp = false;
    }
  }

  statusChangeNotInterested(event: any, statusType: string) {
    if (statusType == 'NotInterested') {
      this.notInterested = true;
      this.callBack = false;
      this.dnd = false;
      this.voicemail = false;
      this.isInvalidNo = false;
    } else if (statusType == 'CallBack') {
      this.notInterested = false;
      this.callBack = true;
      this.dnd = false;
      this.voicemail = false;
      this.isInvalidNo = false;
    } else if (statusType == 'DND') {
      this.notInterested = false;
      this.callBack = false;
      this.dnd = true;
      this.voicemail = false;
      this.isInvalidNo = false;
    } else if (statusType == 'Voicemail') {
      this.notInterested = false;
      this.callBack = false;
      this.dnd = false;
      this.voicemail = true;
      this.isInvalidNo = false;
    } else if (statusType == 'InvalidNo') {
      this.notInterested = false;
      this.callBack = false;
      this.dnd = false;
      this.voicemail = false;
      this.isInvalidNo = true;
    }
  }

  submitRsvp() {
    this.submitted = true;

    if (this.form.invalid) {
      infoNotification('Please fill the name field.');
      return;
    }

    if (this.submitted && this.formRsvp.valid && this.form.valid) {
      this.isAddingRfpt = true;
      const data: AddNewLeadConversion = {
        name: this.form.controls['name'].value,
        email: this.form.controls['email'].value,
        phoneNumber: this.selectedPhone,
        attending: this.formRsvp.controls['attending'].value,
        rsvpType: this.formRsvp.controls['rsvpType'].value,
        status: '1',
        notLookingRadioStatus: '',
        isRsvp: true,
        project: '',
        clientIs: 0,
        planToDo: 0,
        when: new Date().toISOString(),
        cross: this.formRsvp.controls['cross'].value,
        comments: this.formRsvp.controls['comments'].value,
        authDto: GetAuthDetails(),
        isLost: '0',
        isInterested: 1,
      };

      const request$ = this.callCenterHttpService.convertToLead(data);

      request$
        .pipe(
          timeout(4000),
          retry(3),
          switchMap((response) => {
            if (response.isSuccess) {
              successNotification(response.message);
              this.modalService.dismissAll();
              this.saveEndTime();
              this.isAddingRfpt = false;
              return [];
            } else {
              errorNotification(ERROR_MESSAGE);
              this.isAddingRfpt = false;
              return [];
            }
          }),
          catchError((error) => {
            errorNotification(ERROR_MESSAGE);
            this.isAddingRfpt = false;
            this.modalService.dismissAll();
            return [];
          })
        )
        .subscribe();
    }
  }

  submitLost() {
    this.submitted = true;

    if (this.form.invalid) {
      infoNotification('Please fill the name field.');
      return;
    }

    if (this.submitted && this.formLost.valid && this.form.valid) {
      this.isAddingLost = true;
      const data: AddNewLeadConversion = {
        name: this.form.controls['name'].value,
        email: this.form.controls['email'].value,
        phoneNumber: this.selectedPhone,
        attending: '',
        rsvpType: 0,
        status: '1',
        notLookingRadioStatus: this.formNotInterested.controls['status'].value,
        isRsvp: false,
        project: this.formLost.controls['project'].value,
        clientIs: this.formLost.controls['clientIs'].value,
        planToDo: this.formLost.controls['planToDo'].value,
        when: this.formLost.controls['when'].value,
        cross: this.formLost.controls['cross'].value,
        comments: this.formLost.controls['comments'].value,
        authDto: GetAuthDetails(),
        isLost: '1',
        isInterested: 1,
      };

      this.lostConversionSubcription = this.callCenterHttpService
        .convertToLead(data)
        .pipe(
          throttleTime(THOUSAND_THROTTLE_TIME),
          timeout(FOUR_THOUSAND_TIMEOUT_TIME),
          retry(RETRY_COUNT),
          switchMap((response) => {
            if (response.isSuccess) {
              successNotification(response.message);
              this.modalService.dismissAll();
              this.saveEndTime();
              this.isAddingLost = false;
              return [];
            } else {
              this.isAddingLost = false;
              this.modalService.dismissAll();
              errorNotification(ERROR_MESSAGE);
              return [];
            }
          }),
          catchError((error) => {
            this.isAddingLost = false;
            errorNotification(ERROR_MESSAGE);
            this.modalService.dismissAll();
            return [];
          })
        )
        .subscribe();
    }
  }

  submitNotInterested() {
    this.submitted = true;

    if (this.form.invalid) {
      infoNotification('Please fill the name field.');
      return;
    }

    if (this.submitted && this.formNotInterested.valid && this.form.valid) {
      if (this.notInterested) {
        this.formNotInterested.controls['status'].setValue('notinterested');
      } else if (this.callBack) {
        this.formNotInterested.controls['status'].setValue('callback');
      } else if (this.dnd) {
        this.formNotInterested.controls['status'].setValue('dbd');
      } else if (this.voicemail) {
        this.formNotInterested.controls['status'].setValue('voicemail');
      } else if (this.isInvalidNo) {
        this.formNotInterested.controls['status'].setValue('invalidNo');
      }
    }

    if (this.submitted && this.formNotInterested.valid && this.form.valid) {
      this.isAddingNotInterested = true;
      const data: AddNewLeadConversion = {
        name: this.form.controls['name'].value,
        email: this.form.controls['email'].value,
        phoneNumber: this.selectedPhone,
        attending: '',
        rsvpType: 0,
        status: '0',
        notLookingRadioStatus: this.formNotInterested.controls['status'].value,
        isRsvp: false,
        project: '',
        clientIs: 0,
        planToDo: 0,
        when: new Date().toISOString(),
        cross: this.formNotInterested.controls['cross'].value,
        comments: this.formNotInterested.controls['comments'].value,
        authDto: GetAuthDetails(),
        isLost: '0',
        isInterested: 0,
      };

      this.notInterestedConversionSubcription = this.callCenterHttpService
        .convertToLead(data)
        .pipe(
          throttleTime(THOUSAND_THROTTLE_TIME),
          timeout(FOUR_THOUSAND_TIMEOUT_TIME),
          retry(RETRY_COUNT),
          tap((response) => {
            if (response.isSuccess) {
              successNotification('Successfully removed uninterested lead.');
              this.modalService.dismissAll();
              this.saveEndTime();
              this.isAddingNotInterested = false;
              return [];
            } else {
              errorNotification(response.message);
              this.isAddingNotInterested = false;
              this.modalService.dismissAll();
              return [];
            }
          }),
          catchError((err) => {
            this.isAddingNotInterested = false;
            this.modalService.dismissAll();
            errorNotification(ERROR_MESSAGE);
            return [];
          })
        )
        .subscribe();
    }
  }

  getTimeDepedingLink(number: string) {
    console.log(number);
  }

  crossSegmentNotInterested(event: string) {
    if (event == 'yes') {
      this.formNotInterested.controls['cross'].setValue(true);
    } else {
      this.formNotInterested.controls['cross'].setValue(false);
    }
  }

  crossSegment(event: string) {
    if (event == 'yes') {
      this.formRsvp.controls['cross'].setValue(true);
    } else {
      this.formRsvp.controls['cross'].setValue(false);
    }
  }

  crossSegmentLost(event: string) {
    if (event == 'yes') {
      this.formLost.controls['cross'].setValue(true);
    } else {
      this.formLost.controls['cross'].setValue(false);
    }
  }

  loadLeadStatus() {
    const query =
      'SELECT id as _Id, leadstatus as _Value FROM tblLeadStatus WHERE status = 0 AND id NOT IN (19, 18, 17, 16) ORDER BY id ASC';
    this.leadStatus$ = this.commonHttpService
      .getComboBoxData(query)
      .pipe(timeout(5000), retry(3), shareReplay(1));
  }

  loadPlanToDo() {
    const query =
      'SELECT TypeId as _Id, TypeName as _Value FROM tblPlanToDo WHERE status = 0 ORDER BY TypeId DESC';
    this.planToDo$ = this.commonHttpService
      .getComboBoxData(query)
      .pipe(timeout(5000), retry(3), shareReplay(1));
  }

  loadRfpt() {
    const query =
      'SELECT TypeId as _Id, TypeName as _Value FROM tblRsvpType WHERE status = 0 ORDER BY TypeId DESC';
    this.rfpts$ = this.commonHttpService
      .getComboBoxData(query)
      .pipe(timeout(5000), retry(3), shareReplay(1));
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadCallInsignts(newPage);
  }

  ngOnDestroy(): void {
    UnsubscribeHelper(
      this.callEndTimeSubcription,
      this.callGetAllSubcription,
      this.leadConversionSubcription,
      this.RSVPConversionSubcription,
      this.loadCallsSubcription,
      this.lostConversionSubcription,
      this.notInterestedConversionSubcription
    );
  }
}
