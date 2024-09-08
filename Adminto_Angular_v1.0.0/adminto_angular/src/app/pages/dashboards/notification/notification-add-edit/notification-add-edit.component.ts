import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ComboInfo, ComboInfoBank } from '../../shared/models/models';
import { Observable, map, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../../services/common-http.service';
import { NotificationHttpService } from 'src/app/layout/shared/topbar/services/notification-http.service';
import { Router } from '@angular/router';
import { checkForAccess } from '../../shared/methods';
import { MessageResponse, notificationdto } from '../notification-list/models/message';
import { errorNotification, successNotification } from '../../shared/notifications/notification';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { NotificationNewHttpService } from '../services/notification-new-http.service';
import { SendDynamicFormRequest } from '../../staff/models/staff';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { getCurrentDate } from '../../guards/helpers';

@Component({
  selector: 'app-notification-add-edit',
  templateUrl: './notification-add-edit.component.html',
  styleUrls: ['./notification-add-edit.component.scss']
})
export class NotificationAddEditComponent implements OnInit {
  submitted = false;
  submitted2 = false;
  form: FormGroup = new FormGroup({});
  priorityForm : FormGroup = new FormGroup({});
  isEditMode = false;
  itemAdded = false;
  staff$: Observable<ComboInfo[]> = of([]);
  prioritytype$: Observable<ComboInfoBank[]> = of([]);
  notificationinfo$!: MessageResponse;

  get form1() { return this.form.controls; }
  get form2() { return this.priorityForm.controls; }

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService, 
    private notificationHttpService: NotificationNewHttpService, 
    private router: Router, private modalService: NgbModal
    ) { }
    ngOnInit(): void {      
      this.notificationinfo$ = history.state;
      this.loadStaff();
      this.initializeForm();
      this.initializePriorityForm();
      this.loadpriority();
    }
  
    initializeForm() {
      this.form = this.fb.group({
        notify: ['', [Validators.required]],
        date: [getCurrentDate()],
        time: ['', [Validators.required]],
        message: ['', [Validators.required]],
        priorityid: ['', [Validators.required]],
        forwardto: ['', [Validators.required]],
        snoozeon: [getCurrentDate()],
      });
  
  
      if (this.notificationinfo$.id) {
        this.isEditMode = true;
        this.form.patchValue({
          ...this.notificationinfo$,
          date: new Date(this.notificationinfo$.dateAdded).toISOString().split('T')[0],
          message: this.notificationinfo$.message,
          time: this.formatTime(this.notificationinfo$.time),
          priorityid: this.notificationinfo$.priorityId,
          snoozeon: new Date(this.notificationinfo$.snoozeOn).toISOString().split('T')[0],
       
        });

        this.staff$.subscribe({
          next: values => {
            const existingValue = values.findIndex(x => x.textValue == this.notificationinfo$.forwardTo);
            this.form.patchValue({
              notify: values[existingValue].value,
            })
          }
        })
  
        
  
      } else {
        this.isEditMode = false;
      }
    }

    formatTime(inputTime: string): string {
      if (inputTime) {
        const datavalue = inputTime;
        const words = datavalue.split(' ');
           
        return words.join(' ');
      }
      return '';
    }
    
    
    initializePriorityForm() {
      this.priorityForm = this.fb.group({
        catergoryName: ["", [Validators.required]],
        remark: ["", [Validators.required]],
        status: [0, [Validators.required]],
      })
    }
  
    dynamicform(content: TemplateRef<NgbModal>): void {
      this.modalService.open(content, { backdrop: 'static', keyboard: false });
    }
  
    loadStaff() {
      const query =
        "SELECT id as _Id, name as _Value FROM tblstaffs order by name asc";
      this.staff$ = this.commonHttpService.getComboBoxData(query);
    }
  
    async addEditItems() {
  
      this.itemAdded = true;
      try {
        if (this.notificationinfo$.id) {
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
    

    loadpriority() {
      const query =
        "SELECT TypeID as _Id, TypeName as _Value FROM tblprioritytype where Status=0";
      this.prioritytype$ = this.commonHttpService.getComboBoxData(query);
    }

  
    addNewPriority() {
      this.submitted2 = true;
      const values: SendDynamicFormRequest = {
        authDto: GetAuthDetails(),
        dynamicField: "PriorityType",
        ...this.priorityForm.value
      };
  
      this.commonHttpService.addDynamicData(values).subscribe({
        next: response => {
          if (response) {
            successNotification("Success while adding new priority");
            this.modalService.dismissAll();
            this.loadpriority();
          }
        },
        error: response => {
          errorNotification("Error while adding new priority");
        }
      })
    }

    addnotification() {
      const data: notificationdto = {
        authDto: GetAuthDetails(),
        ...this.form.value
      }
  
      this.notificationHttpService.addnotification(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.router.navigateByUrl('/dashboard/notificationlist');
            successNotification(response.message);
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  
    cancel(){
      this.router.navigateByUrl('/dashboard/notificationlist')
    }
    editnotification() {
      const data: MessageResponse = {
        authDto: GetAuthDetails(),
        ...this.form.value,
        id: this.notificationinfo$.id
      }
  
      this.notificationHttpService.updatenotification(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.router.navigateByUrl('/dashboard/notificationlist');
            successNotification(response.message);
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  
  
  }
  
  
  
  
  
  
  

  // ngOnInit(): void {
  //   this.loadStaffs();
  //   this.initializePriorityForm();
  //   this.intializeForm();
  //   // this.editnotificationsInfo = history.state;

  //   // if (this.editnotificationsInfo?.id) {
  //   //   if (this.editnotificationsInfo != null) {
  //   //     this.intializeForm(this.editnotificationsInfo);
  //   //     this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Edit", "notification");
  //   //   }
  //   // } else {
  //   //   this.isAddAllowed$ = checkForAccess(this.commonHttpService, "Add", "notification");
  //   //   this.intializeForm();
  //   // }
  // }
  


  // intializeForm() {
  //     this.form = this.fb.group({
  //       id : [''],
  //       notify : ['', [Validators.required]],
  //       from :['', [Validators.required]],
  //       date : ['', [Validators.required]],
  //       time :  ['', [Validators.required]],
  //       message :  ['', [Validators.required]],
  //       priorityid : ['', [Validators.required]],
  //       addby : [''],
  //       addon : [''],
  //       forwardto : ['', [Validators.required]],
  //       snoozeon : ['', [Validators.required]],
  //     })
  // }





  // onSubmit() {
  //   this.submitted = true;
  //   if (this.form.invalid) {
  //     errorNotification('Please enter required fields');
  //     return;
  //   }

  //   if (!this.isEditMode) {
  //     console.log("executed")
  //     const data: notificationdto = {
  //       authDto: GetAuthDetails(),
  //       ...this.form.value,
  //     };

  //     this.notificationHttpService.addNewNotification(data).subscribe({
  //       next: response => {
  //         if (response.isSuccess) {
  //           successNotification(response.message);
  //           this.form.reset();
  //           this.router.navigateByUrl("/dashboard/notification")
  //         } else {
  //           errorNotification(response.message);
  //         }
  //       }
  //     })
  //   } else {
  //     // const data: notificationsRequest = {
  //     //   authDto: GetAuthDetails(),
  //     //   ...this.form.value,
  //     //   campainid: this.form.controls['campainid'].value,
  //     // };

  //     // this.notificationHttpService.editExistingnotification(data).subscribe({
  //     //   next: response => {
  //     //     if (response.isSuccess) {
  //     //       successNotification(response.message);
  //     //       this.router.navigateByUrl("/dashboard/notification")
  //     //     } else {
  //     //       errorNotification(response.message);
  //     //     }
  //     //   }
  //     // })
  //   }
  // }


  // loadStaffs() {
  //   const query = "SELECT id as _Id, name as _Value FROM tblstaffs  ORDER BY name asc"
  //   this.staff$ = this.commonHttpService.getComboBoxData(query).pipe(
  //     shareReplay(1),
  //   );
  // }


//}
