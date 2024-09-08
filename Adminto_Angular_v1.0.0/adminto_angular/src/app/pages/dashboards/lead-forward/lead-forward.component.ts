import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { LeadForwardViewResponse, LeadsForwardViewData } from './models/leadforward';
import { LeadforwardHttpService } from './services/leadforward-http.service';
import { Router } from '@angular/router';
import { CommonHttpService } from '../services/common-http.service';

@Component({
  selector: 'app-lead-forward',
  templateUrl: './lead-forward.component.html',
  styleUrls: ['./lead-forward.component.scss']
})
export class LeadForwardComponent implements OnInit {
  leadsForwardListInfo$: Observable<Root<LeadsForwardViewData>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;
  isDropDownClicked: boolean = false;
  index: number = 0;
  value: string = '';

  constructor(private leadsForwardHttpService: LeadforwardHttpService, private router: Router, private commonHttpService: CommonHttpService) { }

  ngOnInit(): void {
    this.loadLeadForwards(this.page);
  }

  loadLeadForwards(page: number): void {
    this.leadsForwardHttpService.getAllLeadForwardList(page, this.pageSize).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.leadsForwardListInfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }
    });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadLeadForwards(newPage);
  }

  tableRefresh() {
    this.loadLeadForwards(1);
    this.isDropDownClicked = false;
    this.isPaginating = true;
  }

  editLeadForward(data: LeadForwardViewResponse) {
    this.router.navigateByUrl('/dashboard/leadForwardadd', { state: data });
    //console.log(data);
  }
}
