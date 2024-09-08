import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { CommonHttpService } from '../../common/common-http.service';
import { CallcenterService } from '../Service/callcenter.service';
import { Root } from 'src/app/shared/models/baseResponse';
import { CallCenterInfo, CallCenterRequest } from '../models/callcenter';
import { formatDate, getAuthDetails } from 'src/app/shared/methods/method';
import { errorNotification, infoNotification, successNotification } from 'src/app/core/models/notification';

@Component({
  selector: 'app-call-center-modals',
  templateUrl: './call-center-modals.component.html',
  styleUrls: ['./call-center-modals.component.scss']
})
export class CallCenterModalsComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  leadStatus$: Observable<ComboInfo[]> = of([]);
  leadNo: string = '';
  callCenterInfo$: Observable<Root<CallCenterInfo>> = of();

  constructor(
    public bsModalRef: BsModalRef,
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private callCenterHttpService: CallcenterService
  ) { }

  ngOnInit(): void {
    this.intializeForm();
    this.loadLeadStatus();
    this.loadCallCenterInfo();
  }

  intializeForm() {
    this.form = this.fb.group({
      leadName: [],
      source: [],
      receivedOn: [],
      leadStatus: [null, [Validators.required]],
      remark: ['', [Validators.required]]
    })
  }

  endCall() {
    if (this.form.controls['leadStatus'].value == null) {
      infoNotification("Select a valid lead status");
      return;
    }

    const data: CallCenterRequest = {
      authDto: getAuthDetails(),
      leadNo: this.leadNo,
      leadStatus: this.form.controls['leadStatus'].value,
      remark: this.form.controls['remark'].value,
    }

    this.callCenterHttpService.updateLeadStatus(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.bsModalRef.hide();
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  loadLeadStatus() {
    const query = "SELECT id as _Id, leadstatus as _Value FROM tblLeadStatus WHERE status = 0 ORDER BY id DESC"
    this.leadStatus$ = this.commonHttpService.getComboBoxData(query).pipe(
      shareReplay(1),
    );
  }

  loadCallCenterInfo() {
    if (this.leadNo != '') {
      this.callCenterHttpService.getLeadCallCenterInfo(this.leadNo).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.form.patchValue({
              leadName: response.result.leadName,
              source: response.result.source,
              receivedOn: formatDate(new Date(response.result.receivedOn)),
              leadStatus: response.result.leadStatus
            })
          }
        }
      });
    }
  }
}
