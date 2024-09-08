import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { MeetingupdateService } from './Service/meetingupdate.service';
import { meetsched, remeet, updatemeet } from '../meetingschedule/meeting';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-meetingupdate',
  templateUrl: './meetingupdate.component.html',
  styleUrls: ['./meetingupdate.component.scss']
})
export class MeetingupdateComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  rform: FormGroup = new FormGroup({});
  meet$: Observable<ComboInfo[]> = of([]);
  updatemeetinfo$! : updatemeet;
  remeetinfo$! : remeet;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private commonHttpService: CommonHttpService,
    private meetupdateservices :MeetingupdateService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.initializeFormR();
  }

  initializeForm() {
    this.form = this.fb.group({
      id :[''],
      name :['', [Validators.required]],
      conclusion:['', [Validators.required]],
    });

    this.updatemeetinfo$ = history.state;
      
    if (this.updatemeetinfo$.id) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.updatemeetinfo$,
    });
    } else {
      this.isEditMode = false;
    }
  }
  editupdatemeet() {
    const data: updatemeet = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.updatemeetinfo$.id
    }

    this.meetupdateservices.meetupdate(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/meetingupdatelist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  initializeFormR() {
    this.rform = this.fb.group({
      id :[''],
      meetdate :[getCurrentDate()],
      meettime : ['', [Validators.required]],
      venue : ['', [Validators.required]],
    });
    this.remeetinfo$ = history.state;
      
    if (this.remeetinfo$.id) {
      this.isEditMode = true;
      this.rform.patchValue({
      ...this.remeetinfo$,
      meetdate : new Date(this.remeetinfo$.meetdate).toISOString().split('T')[0],
    });
    } else {
      this.isEditMode = false;
    }
  }

  reschedmeet() {
    const data: remeet = {
      authDto: getAuthDetails(),
      ...this.rform.value,
      id: this.remeetinfo$.id
    }

    this.meetupdateservices.reupdate(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/meetingupdatelist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  
}






