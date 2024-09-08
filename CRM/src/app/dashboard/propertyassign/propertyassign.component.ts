import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { getCurrentDate } from 'src/app/core/models/helpers';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { TypeComponent } from '../type/type.component';
import { environment } from 'src/environments/environment.development';
import { PropertyassignService } from './Service/propertyassign.service';
import { propassignv, propertyassign } from './propertyassign';

@Component({
  selector: 'app-propertyassign',
  templateUrl: './propertyassign.component.html',
  styleUrls: ['./propertyassign.component.scss']
})
export class PropertyassignComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  propassigninfo$!: propassignv;
  salestype$: Observable<ComboInfo[]> = of([]);
  custtype$: Observable<ComboInfo[]> = of([]);
  advtype$: Observable<ComboInfo[]> = of([]);
  isEditMode = false;
  modalRef?: BsModalRef;
  itemAdded = false;
  userImage = '';
  baseUrl = environment.apiUrl;

  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private propertyassignservices: PropertyassignService,
    private modalService: BsModalService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadsale();
    this.loadcustomer();
    this.loadadvnote();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({

      id : [0],
      date :  [getCurrentDate()],
      salesperson : ['', [Validators.required]],
      customerid : ['', [Validators.required]],
      validtill :  [getCurrentDate()],
      advnotno : ['', [Validators.required]],
      description : ['', [Validators.required]],
    });

    this.propassigninfo$ = history.state;

    if (this.propassigninfo$.id) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.propassigninfo$,
        date: new Date(this.propassigninfo$.date).toISOString().split('T')[0],
        validtill: new Date(this.propassigninfo$.validtill).toISOString().split('T')[0],
        advnotno : this.propassigninfo$.advnotno
      });      

      this.salestype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.propassigninfo$.typeName);
          this.form.patchValue({
            salesperson: value[valueOf].value
          })
        }
      });      
      
      this.custtype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.propassigninfo$.custName);
          this.form.patchValue({
            customerid: value[valueOf].value
          })
        }
      });  

    } else {
      this.isEditMode = false;
    }
  }
  
  loadadvnote() {
    const query =
      "SELECT id as _Id, concat(id, ' : ' ,salesby, ' : ' ,customer) as _Value FROM tblAdvPayment order by id asc";
    this.advtype$ = this.commonHttpService.getComboBoxstring(query);
  }
  
  loadsale() {
    const query =
      "SELECT TypeID as _Id, TypeName as _Value FROM tblSales order by TypeID asc";
    this.salestype$ = this.commonHttpService.getComboBoxstring(query);
  }

  loadcustomer() {
    const query =
      "SELECT CustID as _Id, CustName as _Value FROM tblcustomer order by CustID asc";
    this.custtype$ = this.commonHttpService.getComboBoxstring(query);
  }

  updateCheckboxValue(controlName: string, checked: boolean) {
    const value = checked ? 1 : 0;
    this.form.get(controlName)?.setValue(value);
  }

  Navigateto(isSalesby: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isSalesby: isSalesby
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  async addEditItems() {

    this.itemAdded = true;
    try {
      if (this.propassigninfo$.id) {
        await this.editpropassign();
      } else {
        await this.addpropassign();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }

  }

  addpropassign() {

    const data: propertyassign = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.propertyassignservices.addpropassign(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/propertyassignlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editpropassign() {
    const data: propertyassign = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.propassigninfo$.id
    }

    this.propertyassignservices.updatepropassign(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/propertyassignlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


}






