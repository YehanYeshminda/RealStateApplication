import { Component, OnInit } from '@angular/core';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { CallcenterService } from '../Service/callcenter.service';
import { MakeCallCenterAssign, NotinterestedDto } from '../models/callcenter';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-call-center-approve-modal',
  templateUrl: './call-center-approve-modal.component.html',
  styleUrls: ['./call-center-approve-modal.component.scss']
})
export class CallCenterApproveModalComponent implements OnInit {
  email: string = "";
  form: FormGroup = new FormGroup({});

  constructor(private callCenterHttpService: CallcenterService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      remark: ['']
    })
  }

  onScheduleViewing() {
    if (this.email != "") {
      const values: MakeCallCenterAssign = {
        email: this.email,
        remark: this.form.controls['remark'].value,
        authDto: getAuthDetails(),
        status: "Scheduled Viewing"
      };

      this.callCenterHttpService.addNewConvertionToCallInsignt(values).subscribe({
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

  onCallback() {
    if (this.email != "") {
      const values: MakeCallCenterAssign = {
        email: this.email,
        remark: this.form.controls['remark'].value,
        authDto: getAuthDetails(),
        status: "Call Back"
      };

      this.callCenterHttpService.addNewConvertionToCallInsignt(values).subscribe({
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



  onDND() {
    if (this.email != "") {
      const values: NotinterestedDto = {
        email: this.email,
        authDto: getAuthDetails(),
        events: "DND"
      };

      this.callCenterHttpService.newDND(values).subscribe({
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

  notanswer() {
    if (this.email != "") {
      const values: NotinterestedDto = {
        email: this.email,
        authDto: getAuthDetails(),
        events: "notanswering"
      };

      this.callCenterHttpService.newDND(values).subscribe({
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
}
