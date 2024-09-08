import { Component, OnInit } from '@angular/core';
import { LeadsforwardService } from '../leadsforward/Service/leadsforward.service';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { errorNotification, infoNotification, successNotification } from 'src/app/core/models/notification';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonHttpService } from '../common/common-http.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { CallCenterModalsComponent } from './call-center-modals/call-center-modals.component';
import { Root } from 'src/app/shared/models/baseResponse';
import { LeadLogResponseByLeadNo, SheduleCallRequest } from './models/callcenter';
import { CallcenterService } from './Service/callcenter.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { CallInsight } from '../callinsight/models/callinsight';
import { CallinsigntHttpService } from '../callinsight/services/callinsignt-http.service';
import { CallCenterApproveModalComponent } from './call-center-approve-modal/call-center-approve-modal.component';

@Component({
  selector: 'app-callcenter',
  templateUrl: './callcenter.component.html',
  styleUrls: ['./callcenter.component.scss']
})
export class CallcenterComponent implements OnInit {
  leadNos$: Observable<ComboInfoBank[]> = of([]);
  selectedLeadNo: string | null = null;
  preferedMethod: string = 'Select a lead';
  clientName: string = 'Select a lead';
  isClientLoaded: boolean = false;
  form: FormGroup = new FormGroup({});
  scheduleForm: FormGroup = new FormGroup({});
  leadStatus$: Observable<ComboInfo[]> = of([]);
  bsModalRef?: BsModalRef;
  leadNoHistory$: Observable<Root<LeadLogResponseByLeadNo[]>> = of();
  loadedLeads: boolean = false;
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  callInsignts$: Observable<Root<CallInsight[]>> = of();
  selectedCallInsight: number = 0;
  selectedCallInsightPhoneNo!: string;
  selectedEmail: string = "";

  constructor(
    private leadForwardHttpService: LeadsforwardService,
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private callCenterHttpService: CallcenterService,
    private modalService: BsModalService,
    private callInsightService: CallinsigntHttpService
  ) { }

  ngOnInit(): void {
    this.loadLeadNos();
    this.initializeForm();
    this.initializeScheduleForm();
    this.loadLeadStatus();
    this.loadStaff();
    this.loadCallInsignts();
  }

  loadCallInsignts() {
    this.callInsignts$ = this.callInsightService.getAllCallInsigntsForUser();
  }

  openModalWithComponent() {
    if (this.selectedLeadNo == null) {
      infoNotification("Select a Lead No!")
      return;
    }
    const initialState: ModalOptions = {
      initialState: {
        leadNo: this.selectedLeadNo,
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.bsModalRef = this.modalService.show(CallCenterModalsComponent, initialState);
  }

  initializeScheduleForm() {
    this.scheduleForm = this.fb.group({
      ScheuledDate: ['', [Validators.required]],
      ScheuledTime: ['', [Validators.required]],
      description: ['', [Validators.required]],
      AssignedStaff: ['', [Validators.required]],
    });
  }

  initializeForm() {
    this.form = this.fb.group({
      leadStatus: [14, [Validators.required]],
      clientName: [''],
    });
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    )
  }

  onSchedule() {
    const scheduledDate = this.scheduleForm?.get('ScheuledDate')?.value;
    const scheduledTime = this.scheduleForm?.get('ScheuledTime')?.value;
    const combinedDateTime = `${scheduledDate} ${scheduledTime}`;
    const scheduledDateTime = new Date(combinedDateTime);

    if (this.selectedLeadNo != null) {
      const data: SheduleCallRequest = {
        assignedStaff: this.scheduleForm?.get('AssignedStaff')?.value,
        authDto: getAuthDetails(),
        description: this.scheduleForm?.get('description')?.value,
        scheuledTime: scheduledDateTime,
        leadNo: this.selectedLeadNo,
        OriginalDate: scheduledDate,
        OriginalTime: scheduledTime
      };

      this.callCenterHttpService.updateScheduleForLead(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
          } else {
            errorNotification(response.message);
          }
        }
      })
    } else {
      infoNotification("Select a Lead No");
      return;
    }

  }

  loadLeadNoHistory(leadNo: string) {
    this.leadNoHistory$ = this.callCenterHttpService.getLeadLogHistory(leadNo);
    this.loadedLeads = true;
  }

  loadLeadNameAndContactMethod(leadNo: string) {
    if (leadNo != '') {
      this.leadForwardHttpService.loadLeadNoHistoryByLeadId(leadNo).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.form.patchValue({
              leadStatus: response.result.lead.importance,
              clientName: response.result.leadName,
              contactMethod: response.result.contactMethod
            });
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  }

  loadLeadNos() {
    this.leadNos$ = this.leadForwardHttpService.getLeadNoAndIdComboDataAll().pipe(
      shareReplay(1)
    );;
  }

  selectedLead(leadNo: string) {
    this.selectedLeadNo = leadNo;
    this.isClientLoaded = true;
    this.loadLeadNameAndContactMethod(leadNo);
  }

  selectedCallInsignt(data: CallInsight) {
    this.selectedCallInsight = data.id;
    this.selectedCallInsightPhoneNo = data.phoneNo;
    this.selectedEmail = data.email;
    this.form.patchValue({
      clientName: data.firstName
    })
  }

  loadLeadStatus() {
    const query = "SELECT id as _Id, leadstatus as _Value FROM tblLeadStatus WHERE status = 0 ORDER BY id DESC"
    this.leadStatus$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  onCall(callType: string, phone: string) {
    // confirmApproveNotification("Called?").then(response => {
    //   if (response.isConfirmed) {
    //     if (this.selectedEmail != "") {
    //       console.log(this.selectedEmail);
    //       const values: MakeCallCenterAssign = {
    //         email: this.selectedEmail,
    //         authDto: getAuthDetails()
    //       };

    //       this.callCenterHttpService.addNewConvertionToCallInsignt(values).subscribe({
    //         next: response => {
    //           if (response.isSuccess) {
    //             successNotification(response.message);
    //           } else {
    //             errorNotification(response.message);
    //           }
    //         }
    //       })
    //     }
    //   }
    // })

    if (callType == 'phone') {
      this.callInsightService.updateCallIsightStartTime(phone).subscribe({
        next: response => {
          if (response.isSuccess) {

          } else {
            errorNotification(response.message);
          }
        }
      })
    }

    const initialState: ModalOptions = {
      initialState: {
        email: phone
      }
    };

    this.bsModalRef = this.modalService.show(CallCenterApproveModalComponent, initialState);

    this.bsModalRef.onHidden?.subscribe(() => {
      this.callInsightService.updateCallIsightEndTime(phone).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.loadCallInsignts();
          } else  {
            errorNotification(response.message);
          }
        }
      })
    })
  }
}
