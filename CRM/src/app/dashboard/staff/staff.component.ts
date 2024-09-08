import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, infoNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { formatDate, getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { TypeComponent } from '../type/type.component';
import { StaffService } from './Service/staff.service';
import { VStaffList } from './staff';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-staff',
  templateUrl: './staff.component.html',
  styleUrls: ['./staff.component.scss']
})
export class StaffComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  designationtype$: Observable<ComboInfo[]> = of([]);
  imageUrlPassport: string | ArrayBuffer | null | undefined = null;
  imageUrlUserImage: string | ArrayBuffer | null | undefined = null;
  parent$: Observable<ComboInfo[]> = of([]);
  staffinfo$! : VStaffList;
  isEditMode = false;
  itemAdded = false;
  selectedFileName: string = '';
  usericonImage: string = '';
  passportimage: string = '';

  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService,
    private commonHttpService: CommonHttpService,
    private staffservices :StaffService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadDesignation();
    this.loadparent();
    this.initializeForm();
  }


  async addEditItems() {
    if (this.form.controls['passport'].value == '' ) {
      infoNotification("Please select image to proceed")
      return;
    }
    if (this.form.controls['userimage'].value == '' ) {
      infoNotification("Please select image to proceed")
      return;
    }


    this.itemAdded = true;
    try {
      if (this.staffinfo$.Id) {
        await this.editstaff();
      } else {
        await this.addstaff();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }

  }
  initializeForm() {
    this.form = this.fb.group({
      id :[0],
      name :['', [Validators.required]],
      designation :['', [Validators.required]],
      mobileno :['', [Validators.required]],
      parentid :['', [Validators.required]],
      firstname : ['', [Validators.required]],
      lastname : ['', [Validators.required]],
      email : ['', [Validators.required]],
      passport :  ['', []],
      userimage : ['',[]],
      visaIssueDate :[getCurrentDate()],
      password: ['', [Validators.required]]
    });
  
    this.staffinfo$ = history.state;

    
    if (this.staffinfo$.Id) {
      this.isEditMode = true;
      this.form.patchValue({
        name: this.staffinfo$.Name,
        mobileno: this.staffinfo$.Mobileno,
        firstname: this.staffinfo$.FirstName,
        lastname: this.staffinfo$.LastName,
        email: this.staffinfo$.Email,
        id: this.staffinfo$.Id,
        password: this.staffinfo$.Password,
        visaIssueDate : formatDate(new Date(this.staffinfo$.VisaIssuedate)),
        passport: this.staffinfo$.Passport,
        userimage: this.staffinfo$.Userimage
      });

      // this.passportimage += this.staffinfo$.Passport + '.png';
      // this.usericonImage += this.staffinfo$.Userimage + '.png';

      // //this.convertImageUrlToBlob('', '', this.staffinfo$.Passport)
      // //this.onFileChange('', '', this.staffinfo$.Passport)

      // if (this.staffinfo$.Passport) {
      //   this.convertImageUrlToBlob('', '', this.staffinfo$.Passport);
      // }

  
      // if (this.staffinfo$.Userimage) { 
      //   this.convertImageUrlToBlob('', '', this.staffinfo$.Userimage);
      // }
    
      this.designationtype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.staffinfo$.Designation);
          this.form.patchValue({
            designation: value[valueOf].textValue
          });
        }
      });

      this.parent$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.staffinfo$.Parentid);          
          this.form.patchValue({
            parentid: value[valueOf].textValue
          });
          
        }
      });

    } else {
      this.isEditMode = false;
    }
  }

  // convertImageUrlToBlob(event: any, imageUrl: string, inputType: string) {
  //   fetch(imageUrl)
  //     .then((response) => {
  //       if (!response.ok) {
  //         throw new Error('Failed to fetch image');
  //       }
  //       return response.blob();
  //     })
  //     .then((blob) => {
  //       if (inputType === 'Passport') {
  //         this.previewPassportImage(blob as File);
  //       } else if (inputType === 'Userimage') {
  //         this.previewUserImage(blob as File);
  //       }
  //     })
  //     .catch((error) => {
  //       console.error('Error loading image:', error);
  //     });
  // }
  
  

  onFileChange(event: any, inputType: string, newImage?: string) {
    let file = event.target.files[0];

    if (newImage != null) {
      file = newImage;
    }
    
    if (inputType === 'passport') {
      this.previewPassportImage(file);
      this.form.get('passport')?.setValue(file);
    } else if (inputType === 'userimage') {
      this.previewUserImage(file);
      this.form.get('userimage')?.setValue(file);
    }
  
    this.selectedFileName = file ? file.name : '';
  }
  
  previewPassportImage(file: File) {
    const reader = new FileReader();
    reader.onload = () => {
      this.imageUrlPassport = reader.result?.toString();
      console.log(this.imageUrlPassport);
    };
    reader.readAsDataURL(file);
  }
  
  previewUserImage(file: File) {
    const reader = new FileReader();
    reader.onload = () => {
      this.imageUrlUserImage = reader.result?.toString();
      console.log(this.imageUrlUserImage);
    };
    reader.readAsDataURL(file);
  }
  
  

  Navigateto(isDesignation: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isDesignation: isDesignation
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(TypeComponent, initialState);

    this.modalRef.onHidden?.subscribe(() => {
      this.loadDesignation();
    })
  }

  loadDesignation() {
		const query =
			"SELECT TypeID as _Id, TypeName as _Value FROM tblDesignationtype";
		this.designationtype$ = this.commonHttpService.getComboBoxData(query);
	}

  updateCheckboxValue(controlName: string, checked: boolean) {
		const value = checked ? 1 : 0;
		this.form.get(controlName)?.setValue(value);
	}

  loadparent() {
    const query = "SELECT id as _Id, name as _Value FROM tblstaffs"
    this.parent$ = this.commonHttpService.getComboBoxData(query);
  }
  
  
  addstaff() {
    const formData = new FormData();
    const auth = getAuthDetails();
    formData.append('Hash', auth.hash);
    formData.append('id', this.form.value.id);
    formData.append('name', this.form.value.name);
    formData.append('designation', this.form.value.designation);
    formData.append('mobileno', this.form.value.mobileno);
    formData.append('Parentid', this.form.value.parentid);
    formData.append('firstname', this.form.value.firstname);
    formData.append('lastname', this.form.value.lastname);
    formData.append('email', this.form.value.email);
    formData.append('Passport', this.form.get('passport')?.value);
    formData.append('Userimage', this.form.get('userimage')?.value);
    formData.append('VisaIssueDate', this.form.value.visaIssueDate);
    formData.append('password', this.form.value.password);

    this.staffservices.addstaff(formData).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/stafflist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


  editstaff(): void {
    const formData = new FormData();
    const auth = getAuthDetails();
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
    formData.append('Passport', this.form.get('passport')?.value);
    formData.append('Userimage', this.form.get('userimage')?.value);
    formData.append('VisaIssueDate', this.form.value.visaIssueDate);
    formData.append('password', this.form.value.password);

    this.staffservices.updatestaff(formData).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/stafflist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  

  // editstaff() {
  //   const data: staff = {
  //     authDto: getAuthDetails(),
  //     ...this.form.value,
  //     supplierId: this.staffinfo$.id
  //   }

  //   this.staffservices.updatestaff(data).subscribe({
  //     next: response => {
  //       if (response.isSuccess) {
  //         this.router.navigateByUrl('/dashboard/stafflist');
  //         successNotification(response.message);
  //       } else {
  //         errorNotification(response.message);
  //       }
  //     }
  //   })
  // }
  
}






