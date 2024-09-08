import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonHttpService } from '../../services/common-http.service';
import { Observable, of } from 'rxjs';
import { ComboInfo } from '../../shared/models/models';
import { MOBILE_NO_REGEX } from '../../shared/globals';
import { DomSanitizer } from '@angular/platform-browser';
import { errorNotification, infoNotification, successNotification } from '../../shared/notifications/notification';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { StaffHttpService } from '../services/staff-http.service';
import { Router } from '@angular/router';
import { SendDynamicFormRequest, Staff } from '../models/staff';
import { formatDate } from '../../shared/methods';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-staff-add-edit',
  templateUrl: './staff-add-edit.component.html',
  styleUrls: ['./staff-add-edit.component.scss']
})
export class StaffAddEditComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  submitted = false;
  submitted2 = false;
  designationtype$: Observable<ComboInfo[]> = of([]);
  parent$: Observable<ComboInfo[]> = of([]);
  files: File[] = [];
  files2: File[] = [];
  selectedOnImagePassport = false;
  selectedOnImageUser = false;
  editStaffInfo: Staff | null = null;
  isEditMode = false;
  designationForm: FormGroup = new FormGroup({});
  selectedEmailSent = "";
  addingStaff = false;
  @ViewChild('addedStaffPassword') modalStaffAdd!: TemplateRef<any>;


  get form1() { return this.form.controls; }

  constructor(private commonHttpService: CommonHttpService, private fb: FormBuilder, private sanitizer: DomSanitizer, private staffHttpService: StaffHttpService, private router: Router, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.editStaffInfo = history.state;

    this.loadDesignation();
    this.loadparent();
    this.initializeDesignationForm();

    if (this.editStaffInfo?.Id) {
      if (this.editStaffInfo != null) {
        this.initializeForm(this.editStaffInfo);
      }
    } else {
      this.initializeForm();
    }
  }

  initializeForm(data?: Staff) {
    if (data != null) {
      this.isEditMode = true;

      this.form = this.fb.group({
        id :[data.Id],
        name :[data.Name, [Validators.required]],
        designation :[data.Designation, [Validators.required]],
        mobileno :[data.Mobileno, [Validators.required, Validators.pattern(MOBILE_NO_REGEX)]],
        parentid :[data.Parentid, [Validators.required]],
        firstname : [data.FirstName, [Validators.required]],
        lastname : [data.LastName, [Validators.required]],
        email : [data.Email, [Validators.required, Validators.email]],
        passport :  [data.Passport, []],
        userimage : [data.Userimage,[]],
        visaIssueDate :[formatDate(new Date(data.VisaIssuedate))],
        password: [data.Password, [Validators.required]]
      });

      this.designationtype$.subscribe({
        next: values => {
          console.log(values)
          const existingValue = values.findIndex(x => x.textValue == data.Designation);
          this.form.patchValue({
            designation: values[existingValue].value,
          })
        }
      })

      this.parent$.subscribe({
        next: values => {
          console.log(values)
          const existingValue = values.findIndex(x => x.textValue == data.Parentid);
          this.form.patchValue({
            parentid: values[existingValue].value,
          })
        }
      })
    } else {
      this.form = this.fb.group({
        id :[0],
        name :['', [Validators.required]],
        designation :['', [Validators.required]],
        mobileno :['', [Validators.required, Validators.pattern(MOBILE_NO_REGEX)]],
        parentid :['', [Validators.required]],
        firstname : ['', [Validators.required]],
        lastname : ['', [Validators.required]],
        email : ['', [Validators.required]],
        passport :  ['', []],
        userimage : ['',[]],
        visaIssueDate :[],
        // password: ['', [Validators.required]]
      });
    }
  }

  initializeDesignationForm() {
    this.designationForm = this.fb.group({
      catergoryName: ["", [Validators.required]],
      remark: ["", [Validators.required]],
      status: [0, [Validators.required]],
    })
  }

  loadDesignation() {
		const query =
			"SELECT TypeID as _Id, TypeName as _Value FROM tblDesignationtype where Status=0";
		this.designationtype$ = this.commonHttpService.getComboBoxData(query);
	}

  loadparent() {
    const query = "SELECT id as _Id, name as _Value FROM tblstaffs where status = 0"
    this.parent$ = this.commonHttpService.getComboBoxData(query);
  }

  onSelect(event: any) {
    if (this.files.length == 1) {
      errorNotification("Maximum 1 passport image file can be uploaded")
      return;
    }
    
    this.files.push(...event.addedFiles);
    this.selectedOnImagePassport = true;
  }

  onSelectUser(event: any) {
    if (this.files2.length == 1) {
      errorNotification("Maximum 1 user image file can be uploaded")
      return;
    }
    
    this.files2.push(...event.addedFiles);
    this.selectedOnImageUser = true;
  }

  onRemove(event: any) {
    this.files.splice(this.files.indexOf(event), 1);
    this.selectedOnImagePassport = false;
  }

  onRemoveUser(event: any) {
    this.files2.splice(this.files2.indexOf(event), 1);
    this.selectedOnImageUser = false;
  }

  getSize(f: File) {
    const bytes = f.size;
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];

  }

  getSizeUser(f: File) {
    const bytes = f.size;
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];

  }

  getPreviewUrl(f: File) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(encodeURI(URL.createObjectURL(f)));
  }

  getPreviewUrlUser(f: File) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(encodeURI(URL.createObjectURL(f)));
  }

  addstaff() {
    if (this.isEditMode) {
      this.submitted = true;
      this.addingStaff = true;

      if (this.form.valid) {
        if (this.files2.length == 0 || this.files.length == 0) {
          infoNotification("Both images should be uploaded if your are updating staff");
          this.addingStaff = false;
          return;
        }

        this.editstaff();
      }
    } else {
      this.submitted = true;
      if (this.files.length == 0)
      {
        infoNotification("Select a passport image and user image to upload");
        return;
      }

      if (this.files2.length == 0) {
        infoNotification("Select a passport image and user image to upload");
        return;
      } 

      if (this.form.invalid) return;

      this.submitted = true;
      const formData = new FormData();
      const auth = GetAuthDetails();
      formData.append('Hash', auth.hash);
      formData.append('id', this.form.value.id);
      formData.append('name', this.form.value.name);
      formData.append('designation', this.form.value.designation);
      formData.append('mobileno', this.form.value.mobileno);
      formData.append('Parentid', this.form.value.parentid);
      formData.append('firstname', this.form.value.firstname);
      formData.append('lastname', this.form.value.lastname);
      formData.append('email', this.form.value.email);
      formData.append('Passport', this.files[0]);
      formData.append('Userimage', this.files2[0]);
      formData.append('VisaIssueDate', this.form.value.visaIssueDate);
      formData.append('password', this.form.value.password);
  
      formData.forEach((response) => console.log(response))
  
      this.staffHttpService.addstaff(formData).subscribe({
        next: response => {
          if (response.isSuccess) {
            successNotification(response.message);
            this.selectedEmailSent = this.form.value.email;
            this.router.navigateByUrl("/dashboard/staff");
            this.addingStaff = false;
            this.modalService.open(this.modalStaffAdd, { keyboard: false, backdrop: 'static' })
          } else {
            errorNotification(response.message);
          }
        }
      })
    }
  }

  addNewDesignation() {
    this.submitted2 = true;
    const values: SendDynamicFormRequest = {
      authDto: GetAuthDetails(),
      dynamicField: "Designation",
      ...this.designationForm.value
    };

    this.commonHttpService.addDynamicData(values).subscribe({
      next: response => {
        if (response) {
          successNotification("Success while adding new designation");
          this.modalService.dismissAll();
          this.loadDesignation();
        }
      },
      error: response => {
        errorNotification("Error while adding new designation");
      }
    })
  }

  uploadContacts(content: TemplateRef<NgbModal>): void {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  editstaff(): void {
    this.addingStaff = true;
    const formData = new FormData();
    const auth = GetAuthDetails();
    formData.append('Hash', auth.hash);
    formData.append('id', this.form.value.id);
    formData.append('name', this.form.value.name);
    formData.append('designation', this.form.value.designation);
    formData.append('mobileno', this.form.value.mobileno);
    formData.append('Parentid', this.form.value.parentid);
    // formData.append('status', this.form.value.status);
    formData.append('firstname', this.form.value.firstname);
    formData.append('lastname', this.form.value.lastname);
    formData.append('email', this.form.value.email);
    formData.append('Passport', this.files[0]);
    formData.append('Userimage', this.files2[0]);
    formData.append('VisaIssueDate', this.form.value.visaIssueDate);
    formData.append('password', this.form.value.password);

    this.staffHttpService.updatestaff(formData).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
          this.addingStaff = false;
          this.router.navigateByUrl("/dashboard/staff");
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
}
