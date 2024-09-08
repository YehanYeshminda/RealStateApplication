import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { getAuthDetails } from '../shared/method';
import { venderregister } from './Models/vebderregister';
import { errorNotification, successNotification } from '../shared/notifications/notification';
import { VenderService } from './Services/vender.service';
import { CommonHttpService } from '../services/common-http.service';
import { Router } from '@angular/router';
import { ComboInfo } from '../shared/models/models';
import { Observable, of } from 'rxjs';
import { GetAuthDetails } from 'src/app/shared/models/methods';

@Component({
  selector: 'app-vender',
  templateUrl: './vender.component.html',
  styleUrls: ['./vender.component.scss']
})
export class VenderComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  supplierList$: Observable<ComboInfo[]> = of([]);
  venderinfo$! : venderregister;
  submitted = false;
  isEditMode = false;
  get form1() { return this.form.controls; }

  constructor(
    private fb: FormBuilder, 
    private router: Router,
    private commonHttpService: CommonHttpService, 
    private venderregisterservices: VenderService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      supplierId:[],
      supplierName: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phone: ['', [Validators.required, Validators.pattern("^[0-9]{10}$")]],
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
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
  		try {
  			if (this.venderinfo$.supplierId) {
          
  				await this.editSupplier();
  			} else {
          
  				await this.addSupplier();
  			}
  		} catch (error) {
  			console.error('Error processing item:', error);
  		}
  }
  
  addSupplier() {
    const data: venderregister = {
      authDto: GetAuthDetails(),
      ...this.form.value
    }

    this.venderregisterservices.addvender(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/venderlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }


  editSupplier() {
    const data: venderregister = {
      authDto: GetAuthDetails(),
      ...this.form.value,
      supplierId: this.venderinfo$.supplierId,
      staff : 1
      
    }

    this.venderregisterservices.updatevender(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/venderlist');
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


