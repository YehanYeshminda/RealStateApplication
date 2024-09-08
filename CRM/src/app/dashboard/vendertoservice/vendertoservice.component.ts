import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Route, Router } from '@angular/router';
import { VendertoserviceService } from './Service/vendertoservice.service';
import { VtsView, vts } from './vendertoservice';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { TypeComponent } from '../type/type.component';

@Component({
  selector: 'app-vendertoservice',
  templateUrl: './vendertoservice.component.html',
  styleUrls: ['./vendertoservice.component.scss']
})
export class VendertoserviceComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  servicetype$: Observable<ComboInfo[]> = of([]);
  supplier$: Observable<ComboInfo[]> = of([]);
  venderserinfo$!: VtsView;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder,
    private modalService: BsModalService,
    private commonHttpService: CommonHttpService,
    private venderTSservices: VendertoserviceService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadItemTypes();
    this.loadVendors();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      id: [0],
      venderid: ['', [Validators.required]],
      serviceid: ['', [Validators.required]],
      status: [0],
    });

    this.venderserinfo$ = history.state;

    if (this.venderserinfo$.Id) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.venderserinfo$,
      });

      this.servicetype$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.venderserinfo$.TypeName);
          this.form.patchValue({
            venderid: values[existingValue].value,
          })
        }
      })

      this.supplier$.subscribe({
        next: values => {
          const existingValue = values.findIndex(x => x.textValue == this.venderserinfo$.SupplierName);
          this.form.patchValue({
            serviceid: values[existingValue].value,
          })
        }
      })

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

    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  updateCheckboxValue(controlName: string, checked: boolean) {
    const value = checked ? 1 : 0;
    this.form.get(controlName)?.setValue(value);
  }

  loadItemTypes() {
    const query =
      "SELECT TypeID as _Id, TypeName as _Value FROM tblServicetype where Status='1' order by TypeName asc";
    this.servicetype$ = this.commonHttpService
      .getComboBoxData(query)
      .pipe(shareReplay(1));
  }

  loadVendors() {
    const query = "SELECT SupplierID as _Id, SupplierName as _Value FROM tblsupplier ORDER BY SupplierID DESC"
    this.supplier$ = this.commonHttpService.getComboBoxData(query);
  }

  async addEditItems() {
    this.itemAdded = true;

    try {
      if (this.venderserinfo$.Id) {
        console.log('edit')
        await this.editVTS();
      } else {
        console.log('add')

        await this.addVTS();
      }

      this.itemAdded = false;
    } catch (error) {
      console.error('Error processing item:', error);
      this.itemAdded = false;
    }
  }

  addVTS() {
    const data: vts = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.venderTSservices.addVTS(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/vendertoservicelist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editVTS() {
    const data: vts = {
      authDto: getAuthDetails(),
      ...this.form.value,
      supplierId: this.venderserinfo$.Id
    }

    this.venderTSservices.updateVTS(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/vendertoservicelist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

}






