import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Observable, of } from 'rxjs';
import { CommonHttpService } from '../common/common-http.service';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Router } from '@angular/router';
import { Viourtn, iouRtn } from './iouRtn';
import { IoureturnService } from './Service/ioureturn.service';
import { getCurrentDate } from 'src/app/core/models/helpers';

@Component({
  selector: 'app-ioureturn',
  templateUrl: './ioureturn.component.html',
  styleUrls: ['./ioureturn.component.scss']
})
export class IoureturnComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  iou$: Observable<ComboInfo[]> = of([]);
  iouinfo$! : Viourtn;
  isEditMode = false;
  itemAdded = false;

  constructor(
    private fb: FormBuilder, 
    private commonHttpService: CommonHttpService,
    private ioureturnServices :IoureturnService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadiou();
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      rtnid :[0],
      iouid  :['', [Validators.required]],
      desc :['',[Validators.required]],
      retnon  :[getCurrentDate()],
    });
  
    this.iouinfo$ = history.state;
      
    if (this.iouinfo$.rtnid) {
      this.isEditMode = true;
      this.form.patchValue({
        ...this.iouinfo$,
        retnon: new Date(this.iouinfo$.retnon).toISOString().split('T')[0],
      });      

      // this.iou$.subscribe({
      //   next: value => {
      //     const valueOf = value.findIndex(x => x.textValue == this.iouinfo$.iouid.toString());
      //     this.form.patchValue({
      //       iouid: value[valueOf].value
      //     })
      //   }
      // });

    } else {
      this.isEditMode = false;
    }
  }

  loadiou() {
    const query = "SELECT id as _Id, id as _Value FROM tblIOU where returned = 0 ORDER BY id ASC"
    this.iou$ = this.commonHttpService.getComboBoxData(query);
  }

  async addEditItems() {
  		this.itemAdded = true;

  		try {
  			if (this.iouinfo$.rtnid) {
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
    const data: iouRtn = {
      authDto: getAuthDetails(),
      ...this.form.value
    }

    this.ioureturnServices.addiouRtn(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/ioureturnlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  editiou() {
    const data: iouRtn = {
      authDto: getAuthDetails(),
      ...this.form.value,
      rtnid: this.iouinfo$.rtnid
    }

    this.ioureturnServices.updateiouRtn(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.router.navigateByUrl('/dashboard/ioureturnlist');
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
  
}






