import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Route, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { customer } from './customer';
import { CustomerService } from './Service/customer.service';
import { CityComponent } from '../city/city.component';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  customertype$: Observable<ComboInfo[]> = of([]);
  customer$: Observable<ComboInfo[]> = of([]);
  customerinfo$! : customer;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService,
    private commonHttpService: CommonHttpService,
    private customerservices :CustomerService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadCity();
    this.loadparent();
  }

  initializeForm() {
    this.form = this.fb.group({

      custId : [0],
      custName : ['', [Validators.required]],
      custAddress : ['', [Validators.required]],
      custCity : ['', [Validators.required]],
      custMobile : ['', [Validators.required]],
      custPhone : ['', [Validators.required]],
      email : ['', [Validators.required]],
      contPerson : ['', [Validators.required]],
      creditLimit : ['', [Validators.required]],
      creditDays : ['', [Validators.required]],
      status : [0],
      creditPeriod : ['', [Validators.required]],
      remarks : ['', [Validators.required]],
      cardNo : ['', [Validators.required]],
      vatNo : ['', [Validators.required]],
      totRetCheque : [0, [Validators.required]],
    });
  
    this.customerinfo$ = history.state;
      
    if (this.customerinfo$.custId) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.customerinfo$,
    });
    } else {
      this.isEditMode = false;
    }
  }

  Navigateto(isType: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isType: isType
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(CityComponent, initialState);
  }

  loadCity() {
		const query =
			"SELECT TypeID as _Id, TypeName as _Value FROM tblCitytype where status=1 order by TypeName asc";
		this.customertype$ = this.commonHttpService
			.getComboBoxData(query)
			.pipe(shareReplay(1));
	}

  updateCheckboxValue(controlName: string, checked: boolean) {
		const value = checked ? 1 : 0;
		this.form.get(controlName)?.setValue(value);
	}

  loadparent() {
    const query = "SELECT id as _Id, name as _Value FROM tblcustomers ORDER BY name ASC"
    this.customer$ = this.commonHttpService.getComboBoxData(query);
  }

  async addEditItems() {
  		this.itemAdded = true;

  		try {
  			if (this.customerinfo$.custId) {
  				await this.editcustomer();
  			} else {
  				await this.addcustomer();
  			}

  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
  }
  
  addcustomer() {
    const data: customer = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.customerservices.addcustomer(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/customerlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editcustomer() {
    const data: customer = {
      authDto: getAuthDetails(),
      ...this.form.value,
      supplierId: this.customerinfo$.custId
    }

    this.customerservices.updatecustomer(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/customerlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  
}






