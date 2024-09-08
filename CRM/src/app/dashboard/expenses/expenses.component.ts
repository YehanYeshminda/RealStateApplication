import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { ExpensesService } from './Service/expenses.service';
import { expense, expenseview } from './expense';
import { getCurrentDate } from 'src/app/core/models/helpers';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { TypeComponent } from '../type/type.component';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.scss']
})
export class ExpensesComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  expenseinfo$! : expenseview;
  vendertype$: Observable<ComboInfo[]> = of([]);
  expensetype$: Observable<ComboInfo[]> = of([]);
  branchtype$: Observable<ComboInfo[]> = of([]);
  usertype$: Observable<ComboInfo[]> = of([]);
  isEditMode = false;
  modalRef?: BsModalRef;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private commonHttpService: CommonHttpService,
    private expensesservices : ExpensesService,
    private modalService: BsModalService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.getLastNo();
    this.loadSupplier();
    this.loadExpenseAccount();
    this.loadBranch();
    this.loadUser();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      id :[''],
      vDate :[getCurrentDate(), [Validators.required]],
      supplierId :['', [Validators.required]],
      mainCatId :[''],
      subcatId :[''],
      description :['', [Validators.required]],
      cashPaid :['', [Validators.required]],
      chequePaid :['', [Validators.required]],
      chequeNo :['', [Validators.required]],
      status :[0],
      authBy :['', [Validators.required]],
      receiptNo :['', [Validators.required]],
      accountId :['', [Validators.required]],
      rDate  :[getCurrentDate()],
      uniqueId :[''],
      totalValue :['', [Validators.required]],
      vatp :['', [Validators.required]],
      vat :['', [Validators.required]],
      netTotal :['', [Validators.required]],
      brid :['', [Validators.required]],
      used :['', [Validators.required]],
      chequebal :['', [Validators.required]],
      duebal :['', [Validators.required]],
			paid: [0],
    });

    this.form.controls['accountId'].valueChanges.subscribe({
			next: (response) => {
				this.loadMaincat(response);
			},
		});
  
    this.expenseinfo$ = history.state;
      
    if (this.expenseinfo$.id) {
      this.isEditMode = true;
      console.log(this.expenseinfo$);
      this.form.patchValue({
      ...this.expenseinfo$,
        vDate: new Date(this.expenseinfo$.vDate).toISOString().split('T')[0],
        rDate: new Date(this.expenseinfo$.rDate).toISOString().split('T')[0],
      });

      this.vendertype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.expenseinfo$.supplierName);
          this.form.patchValue({
            supplierId: value[valueOf].value
          })
        }
      });

      this.expensetype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.expenseinfo$.accountId);
          this.form.patchValue({
            accountId: value[valueOf].value
          });
        }
      });

      this.usertype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.expenseinfo$.username);
          this.form.patchValue({
            authBy: value[valueOf].value
          })
        }
      });

      this.branchtype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.expenseinfo$.branchName);
          this.form.patchValue({
            brid: value[valueOf].value
          })
        }
      });

    } else {
      this.isEditMode = false;
    }
  }

  

  getLastNo() {
    if (!this.isEditMode) {
      this.commonHttpService.getGetLastValueFromValue('EX', 'tblBranchControl', 'VoucherNo').subscribe({
        next: response => {
          this.form.patchValue({
            id: response.lastValue
          })
        }
      });
    }
  }

	loadSupplier() {
		const query =
			"SELECT SupplierID as _Id, SupplierName as _Value FROM tblSupplier order by SupplierName asc";
		this.vendertype$ = this.commonHttpService.getComboBoxData(query);
	}

  loadBranch() {
		const query =
			"SELECT BrID as _Id, BranchName as _Value FROM tblbranches order by BranchName asc";
		this.branchtype$ = this.commonHttpService.getComboBoxData(query);
	}

  loadExpenseAccount() {
		const query =
			"SELECT ID as _Id,CONCAT(SubCategory,' - ',MainCategory, ' - ',ID ) as _Value FROM vExepnsesAccount order by ID asc";
		this.expensetype$ = this.commonHttpService.getComboBoxData(query);
	}

  loadUser() {
		const query =
			"SELECT userid as _Id, username as _Value FROM tblusers order by username asc";
		this.usertype$ = this.commonHttpService.getComboBoxData(query);
	}


  Navigateto(isExpense: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isExpense: isExpense
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  NavigateMainto(isMain: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isMain: isMain
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  NavigateSubto(isSub: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isSub: isSub
      },
      class: 'modal-lg',
      backdrop: 'static',
    };
    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  async addEditItems() {
    
  		this.itemAdded = true;
  		try {
  			if (this.expenseinfo$.id) {
  				await this.editexpense();
  			} else {
  				await this.addexpense();
  			}

  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
      
  }
  
  loadMaincat(id: number) {
		if (!id) return;

		this.expensesservices.getMaincatById(id).subscribe({
			next: (response) => {
				this.form.patchValue({
					mainCatId: response.mainCatergory,
          subcatId: response.subCatergory,
				});
			},
		});
	}

  addexpense() {
    const data: expense = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.expensesservices.addexpense(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/expenselist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editexpense() {
    const data: expense = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.expenseinfo$.id
    }

    this.expensesservices.updateexpense(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/expenselist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  

}






