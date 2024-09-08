import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of, shareReplay } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { iou, viou } from './iou';
import { IouService } from './Service/iou.service';
import { TypeComponent } from '../type/type.component';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-iou',
  templateUrl: './iou.component.html',
  styleUrls: ['./iou.component.scss']
})
export class IouComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  modalRef?: BsModalRef;
  issuetotype$: Observable<ComboInfo[]> = of([]);
  approvedbytype$: Observable<ComboInfo[]> = of([]);
  iou$: Observable<ComboInfo[]> = of([]);
  iouinfo$! : viou;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private modalService: BsModalService,
    private commonHttpService: CommonHttpService,
    private iouservices :IouService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadissuedto();
    this.loadapprovedby();
    this.loadparent();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      id :[0],
      date :[getCurrentDate()],
      issueto :['', [Validators.required]],
      reason :['', [Validators.required]],
      returnon :[getCurrentDate()],
      approvedby :['', [Validators.required]],
      value :['', [Validators.required]],
    });
  
    this.iouinfo$ = history.state;
      
    if (this.iouinfo$.id) {
      this.isEditMode = true;
      this.form.patchValue({
      ...this.iouinfo$,
       date: new Date(this.iouinfo$.date).toISOString().split('T')[0],
      returnon: new Date(this.iouinfo$.returnon).toISOString().split('T')[0],
      });      

      this.issuetotype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.iouinfo$.typeName);
          this.form.patchValue({
            issueto: value[valueOf].value
          })
        }
      });   

      this.approvedbytype$.subscribe({
        next: value => {
          const valueOf = value.findIndex(x => x.textValue == this.iouinfo$.username);
          this.form.patchValue({
            approvedby: value[valueOf].value
          })
        }
      });

    } else {
      this.isEditMode = false;
    }
  }

  Navigate(isIssuedTo: boolean) {
    const initialState: ModalOptions = {
      initialState: {
        isIssuedTo: isIssuedTo
      },
      class: 'modal-lg',
      backdrop: 'static',
    };

    this.modalRef = this.modalService.show(TypeComponent, initialState);
  }

  loadissuedto() {
		const query =
			"SELECT TypeID as _Id, TypeName as _Value FROM tblIssuedTo where status=1 order by TypeName asc";
		this.issuetotype$ = this.commonHttpService
			.getComboBoxData(query)
			.pipe(shareReplay(1));
	}

  loadapprovedby() {
		const query =
			"SELECT userid as _Id, username as _Value FROM tblusers order by username asc";
		this.approvedbytype$ = this.commonHttpService
			.getComboBoxData(query)
			.pipe(shareReplay(1));
	}

  updateCheckboxValue(controlName: string, checked: boolean) {
		const value = checked ? 1 : 0;
		this.form.get(controlName)?.setValue(value);
	}

  loadparent() {
    const query = "SELECT id as _Id, name as _Value FROM tblious ORDER BY name ASC"
    this.iou$ = this.commonHttpService.getComboBoxData(query);
  }

  async addEditItems() {
  		this.itemAdded = true;

  		try {
  			if (this.iouinfo$.id) {
  				await this.editiou();
  			} else {
  				await this.addiou();
  			}

  			this.itemAdded = false;
  		} catch (error) {
  			console.error('Error processing item:', error);
  			this.itemAdded = false;
  		}
  }
  
  addiou() {
    const data: iou = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.iouservices.addiou(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/ioulist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editiou() {
    const data: iou = {
      authDto: getAuthDetails(),
      ...this.form.value,
      id: this.iouinfo$.id
    }

    this.iouservices.updateiou(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/ioulist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  
}






