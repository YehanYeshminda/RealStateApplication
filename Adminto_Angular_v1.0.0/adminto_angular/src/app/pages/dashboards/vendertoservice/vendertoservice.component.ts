import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../services/common-http.service';
import { ComboInfo } from '../shared/models/models';
import { successNotification, errorNotification } from '../shared/notifications/notification';
import { VtsView, vts } from './Modals/vts';
import { VendertoserviceService } from './Services/vendertoservice.service';
import { SendDynamicFormRequest } from '../staff/models/staff';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-vendertoservice',
  templateUrl: './vendertoservice.component.html',
  styleUrls: ['./vendertoservice.component.scss']
})
export class VendertoserviceComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  servicetype$: Observable<ComboInfo[]> = of([]);
  supplier$: Observable<ComboInfo[]> = of([]);
  //venderserinfo$!: VtsView;
  isEditMode = false;
  editvtsInfo: VtsView | null = null;
  submitted = false;
  submitted2 = false;
  typeForm: FormGroup = new FormGroup({});

  get form1() { return this.form.controls; }
  
  constructor(
    private fb: FormBuilder,
    private commonHttpService: CommonHttpService,
    private venderTSservices: VendertoserviceService,
    private router: Router,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {

    this.editvtsInfo = history.state;
    
    this.initializetypeForm();
    this.loadVendors();
    this.loadtype();

    if (this.editvtsInfo?.Id) {
      if (this.editvtsInfo != null) {
        this.initializeForm(this.editvtsInfo);
      }
    } else {
      this.initializeForm();
    }
  }

  initializetypeForm() {
    this.typeForm = this.fb.group({
      catergoryName: ["", [Validators.required]],
      remark: ["", [Validators.required]],
      status: [0, [Validators.required]],
    })
  }

  initializeForm(data?: VtsView) {
    if (data != null) {
      this.isEditMode = true;
  
      this.form = this.fb.group({
          id: [data.Id],
          venderid: [data.SupplierName, [Validators.required]],
          serviceid: [data.TypeName, [Validators.required]],
          status: [data.Status],
      });
  
      if (this.editvtsInfo) {
        this.servicetype$.subscribe({
          next: values => {
            const existingValue = values.findIndex(x => x.textValue === this.editvtsInfo?.TypeName);
            this.form.patchValue({
              serviceid: values[existingValue].value,
            });
          }
        });

        this.supplier$.subscribe({
          next: values => {
            const existingValue = values.findIndex(x => x.textValue === this.editvtsInfo?.SupplierName);
            this.form.patchValue({
              venderid: values[existingValue].value,
            });
          }
        });
      }
    } 
    else 
    {
      this.form = this.fb.group({
        id: [0],
        venderid: ['', [Validators.required]],
        serviceid: ['', [Validators.required]],
        status: [0],
      });
    }
  }


  loadtype() {
		const query =
			"SELECT TypeID as _Id, TypeName as _Value FROM tblServicetype";
		this.servicetype$ = this.commonHttpService.getComboBoxData(query);
	}

  updateCheckboxValue(controlName: string, checked: boolean) {
    const value = checked ? 1 : 0;
    this.form.get(controlName)?.setValue(value);
  }

  uploaddata(content: TemplateRef<NgbModal>): void {
    this.modalService.open(content, { backdrop: 'static', keyboard: false });
  }

  loadVendors() {
    const query = "SELECT SupplierID as _Id, SupplierName as _Value FROM tblsupplier ORDER BY SupplierID DESC"
    this.supplier$ = this.commonHttpService.getComboBoxData(query);
  }

  async addEditItems() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
  		try {
  			if (this.editvtsInfo?.Id) {          
  				await this.editVTS();
  			} else {          
  				await this.addVTS();
  			}
  		} catch (error) {
  			console.error('Error processing item:', error);
  		}
  }

  addVTS() {
    const data: vts = {
      authDto: GetAuthDetails(),
      ...this.form.value
    }

    this.venderTSservices.addVTS(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/vtslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editVTS() {
    const data: vts = {
      authDto: GetAuthDetails(),
      ...this.form.value,
      Id: this.editvtsInfo?.Id
    }

    this.venderTSservices.updateVTS(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/vtslist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  addNewservice() {
    this.submitted2 = true;
    const values: SendDynamicFormRequest = {
      authDto: GetAuthDetails(),
      dynamicField: "Service",
      ...this.typeForm.value
    };

    this.commonHttpService.addDynamicData(values).subscribe({
      next: response => {
        if (response) {
          successNotification("Success while adding new service");
          this.modalService.dismissAll();
          this.loadtype();
        }
      },
      error: response => {
        errorNotification("Error while adding new service");
      }
    })
  }

}
