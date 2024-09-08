import { Component, OnInit } from '@angular/core';
import { LeadLogHttpService } from '../services/lead-log-http.service';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { LeadLogResponse } from '../../lead-forward/models/leadforward';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LeadLogRequest, LeadLogViewResponse } from '../models/log';
import { GetAuthDetails } from 'src/app/shared/models/methods';

@Component({
  selector: 'app-view-lead-log',
  templateUrl: './view-lead-log.component.html',
  styleUrls: ['./view-lead-log.component.scss']
})
export class ViewLeadLogComponent implements OnInit {
  leadLogs$: Observable<Root<LeadLogViewResponse[]>> = of();
  form: FormGroup = new FormGroup({});
  submitted = false;
  loaded = false;

  constructor(private leadLogHttpService: LeadLogHttpService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      leadNo: ['', [Validators.required]]
    })
  }

  onSubmit() {
    this.submitted = true;

    if (this.form.valid && this.submitted) {
      const data: LeadLogRequest = {
        authDto: GetAuthDetails(),
        leadNo: this.form.controls['leadNo'].value
      };
      
      this.leadLogHttpService.leadLogResponse(data).subscribe({
        next: response => {
          if (response.isSuccess) {
            this.leadLogs$ = of(response);
            this.submitted = false;
            this.loaded = true;
          }
        }
      })
    }
  }
}
