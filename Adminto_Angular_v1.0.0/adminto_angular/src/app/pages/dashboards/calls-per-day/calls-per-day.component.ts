import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UpdateCountHttpService } from './services/update-count-http.service';
import { errorNotification, successNotification } from '../shared/notifications/notification';
import { Observable, of, shareReplay } from 'rxjs';
import { ComboInfoBank } from '../shared/models/models';
import { LeadforwardHttpService } from '../lead-forward/services/leadforward-http.service';

@Component({
  selector: 'app-calls-per-day',
  templateUrl: './calls-per-day.component.html',
  styleUrls: ['./calls-per-day.component.scss']
})
export class CallsPerDayComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  staffNos$: Observable<ComboInfoBank[]> = of([]);
  submitted: boolean = false;
  listOfValues: any = [
    {value: "Lead", textValue: "Leads"},
    {value: "Calls", textValue: "Calls"},
  ];

  get form1() { return this.form.controls; }

  constructor(private fb: FormBuilder, private updateCountPerDayHttpService: UpdateCountHttpService, private leadForwardHttpService: LeadforwardHttpService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadStaff();
  }

  initializeForm() {
    this.form = this.fb.group({
      count: ['', [Validators.required]],
      type: ['Lead', [Validators.required]],
      staffId: [1, [Validators.required]],
    })
  }

  loadStaff() {
    this.staffNos$ = this.leadForwardHttpService.getStaffComoboId().pipe(
      shareReplay(1)
    )
  }

  updateDailyCount() {
    this.updateCountPerDayHttpService.updateCount(this.form.controls['count'].value, this.form.controls['type'].value, this.form.controls['staffId'].value).subscribe({
      next: response => {
        if (response.isSuccess) {
          successNotification(response.message);
        } else {
          errorNotification(response.message);
        }
      }
    })
  }
}
