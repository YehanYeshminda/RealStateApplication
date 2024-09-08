import { Component, OnInit } from '@angular/core';
import { ArchivedLeadHttpService } from '../services/archived-lead-http.service';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { ArchivedLeadListDtoRequest, ArchivedLeadsAll } from '../models/archivedlead';
import { GetAuthDetails } from 'src/app/shared/models/methods';

@Component({
  selector: 'app-archived-leads',
  templateUrl: './archived-leads.component.html',
  styleUrls: ['./archived-leads.component.scss']
})
export class ArchivedLeadsComponent implements OnInit {
  archivedLeads$: Observable<Root<ArchivedLeadsAll>> = new Observable();
  totalData!: number;
  page = 1;
  pageSize = 20;
  isPaginating: boolean = false;

  constructor(private archivedLeadsHttpService: ArchivedLeadHttpService) { }

  ngOnInit(): void {
    this.loadArchviedLeads();
  }

  tableRefresh() {}

  loadArchviedLeads() {
    const data: ArchivedLeadListDtoRequest = {
      authDto: GetAuthDetails(),
      pageSize: this.pageSize,
      page: this.page
    }

    this.archivedLeads$ = this.archivedLeadsHttpService.getAllArchivedLeads(data);
  }

  onPageChange(event: any) {}
}
