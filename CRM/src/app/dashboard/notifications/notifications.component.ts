import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { notification, vNofitication } from './notifications';
import { NotificationsService } from './Service/notifications.service';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  notificationinfo$!: vNofitication;
  stafftype$: Observable<ComboInfo[]> = of([]);
  forwardtype$: Observable<ComboInfo[]> = of([]);
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private notificationsservices: NotificationsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadStaff();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      notify: ['', [Validators.required]],
      date: [getCurrentDate()],
      time: ['', [Validators.required]],
      message: ['', [Validators.required]],
      priorityid: ['', [Validators.required]],
      forwardto: ['', [Validators.required]],
    });

    this.notificationinfo$ = history.state;

    if (this.notificationinfo$.Id) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.notificationinfo$,
        date: new Date(this.notificationinfo$.Date).toISOString().split('T')[0],
        message: this.notificationinfo$.Message,
        time: this.notificationinfo$.Time,
        priorityid: this.notificationinfo$.prority
      });

      this.stafftype$.subscribe({
        next: values => {
          console.log(values);
          console.log(this.notificationinfo$.Name)
          const existingValue = values.findIndex(x => x.textValue == this.notificationinfo$.Name);
          this.form.patchValue({
            notify: values[existingValue].value,
          })
        }
      })

      this.forwardtype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.notificationinfo$.ForwardTo);
          this.form.patchValue({
            forwardto: values[existingValue].value,
          })
        }
      })

    } else {
      this.isEditMode = false;
    }
  }


  loadStaff() {
    const query =
      "SELECT id as _Id, name as _Value FROM tblstaffs order by name asc";
    this.stafftype$ = this.commonHttpService.getComboBoxData(query);
    this.forwardtype$ = this.commonHttpService.getComboBoxData(query);
  }

  async addEditItems() {

    this.itemAdded = true;
    try {
      if (this.notificationinfo$.Id) {
        await this.editnotification();
      } else {
        await this.addnotification();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }

  }

  addnotification() {
    const data: notification = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.notificationsservices.addnotification(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/notificationslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editnotification() {
    const data: notification = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.notificationinfo$.Id
    }

    this.notificationsservices.updatenotification(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/notificationslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


}






