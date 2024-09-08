import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { venderregister } from './venderregister';
import { VenderService } from './Service/venderregister.service';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';

@Component({
  selector: 'app-venderregister',
  templateUrl: './venderregister.component.html',
  styleUrls: ['./venderregister.component.scss']
})
export class VenderregisterComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  supplierList$: Observable<ComboInfo[]> = of([]);
  venderinfo$! : venderregister;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private router: Router,
    private commonHttpService: CommonHttpService, 
    private venderregisterservices: VenderService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    //this.loadVendors();
    // this.loadBanks();
  }

  initializeForm() {
    this.form = this.fb.group({
      supplierId:[],
      supplierName: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      fax: ['', [Validators.required]],
      email: ['', [Validators.required]],
      mobile: ['', [Validators.required]],
      creditPeriod: ['', [Validators.required]],
      staff: [1, [Validators.required]],
      status: [1, [Validators.required]],
      cid: ["1", [Validators.required]],
      vatNo: ['', [Validators.required]],
    });
  
    this.venderinfo$ = history.state;
      
    if (this.venderinfo$.supplierId) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.venderinfo$,
    });
    } else {
      this.isEditMode = false;
    }
  }

  // loadVendors() {
  //   const query = "SELECT SupplierID as _Id, SupplierName as _Value FROM tblsupplier WHERE status = 0 ORDER BY SupplierID DESC"
  //   this.supplierList$ = this.commonHttpService.getComboBoxData(query);
  // }

  async addEditItems() {
  		this.itemAdded = true;

  		try {
  			if (this.venderinfo$.supplierId) {
          console.log('edit')
  				await this.editSupplier();
  			} else {
          console.log('add')

  				await this.addSupplier();
  			}

  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
  }
  
  addSupplier() {
    const data: venderregister = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.venderregisterservices.addvender(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


  editSupplier() {
    const data: venderregister = {
      authDto: getAuthDetails(),
      ...this.form.value,
      supplierId: this.venderinfo$.supplierId
    }

    this.venderregisterservices.updatevender(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/venderregisterlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  // loadBanks() {
  //   this.bankList$ = this.commonHttpService.getBankNameAndIds();
  // }
}






