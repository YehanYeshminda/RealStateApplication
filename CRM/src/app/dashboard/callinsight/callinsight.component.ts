import { Component, OnInit } from '@angular/core';
import { Observable, of, shareReplay, tap } from 'rxjs';
import { Root } from 'src/app/shared/models/baseResponse';
import { CallinsigntHttpService } from './services/callinsignt-http.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Bulkassign, CallInsight, CallInsightRequest } from './models/callinsight';
import { Router } from '@angular/router';
import { errorNotification, successNotification } from 'src/app/core/models/notification';
import { LeadsforwardService } from '../leadsforward/Service/leadsforward.service';
import { ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { getAuthDetails } from 'src/app/shared/methods/method';

@Component({
  selector: 'app-callinsight',
  templateUrl: './callinsight.component.html',
  styleUrls: ['./callinsight.component.scss']
})
export class CallinsightComponent implements OnInit {
  callInsignt$: Observable<Root<CallInsight[]>> = of();
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  form: FormGroup = new FormGroup({});
  callInsigntIds: number[] = [];

  constructor(
    private callInsightHttpService: CallinsigntHttpService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private leadForwardHttpService: LeadsforwardService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadInsights().subscribe();
    this.loadStaff();
  }

  initializeForm() {
    this.form = this.fb.group({
      assignStaff: [0, [Validators.required]],
      counts : [0, [Validators.required]],
    })
  }

  onSubmit() {
    const data: CallInsightRequest = {
      assignStaff: this.form.controls['assignStaff'].value,
      callInsigntIds: this.callInsigntIds,
      authDto: getAuthDetails()
    }

    this.callInsightHttpService.assignCallInsights(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }

  oncount() {
    const numberToAssign = this.form.get('counts')?.value; 
    const data: Bulkassign = {
      assignStaff: this.form.controls['assignStaff'].value,
      authDto: getAuthDetails(),
      numberOfItemsToAssign: numberToAssign,
    };
  
    this.callInsightHttpService.assignbulk(data).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    });
  }
  

  loadInsights(): Observable<Root<CallInsight[]>> {
    this.spinner.show();
    return this.callInsightHttpService.getAllCallInsignts().pipe(
      tap((data) => {
        if (data.isSuccess) {
          this.callInsignt$ = of(data);
          this.spinner.hide();
        } else if (!data.isSuccess && data.message == "Invalid Hash") {
          this.router.navigateByUrl('/');
          this.spinner.hide();
        } else if (!data.isSuccess) {
          errorNotification(data.message);
          this.spinner.hide();
        }
      }),
    );
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    )
  }

  changeStatus(id: number) {
    const index = this.callInsigntIds.indexOf(id);

    if (index === -1) {
      this.callInsigntIds.push(id);
    } else {
      this.callInsigntIds.splice(index, 1); 
    }
  }

}
