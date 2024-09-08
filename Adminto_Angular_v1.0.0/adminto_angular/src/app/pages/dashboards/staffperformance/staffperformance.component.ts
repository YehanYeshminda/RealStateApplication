import { Component, OnInit } from '@angular/core';
import { PaginationResult, StaffPerformance, StaffPerformanceRequest } from './Models/staffperfo';
import { Root } from 'src/app/shared/models/base';
import { Observable, of } from 'rxjs';
import { StaffperformanceService } from './Services/staffperformance.service';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { FormBuilder, FormGroup } from '@angular/forms';
import { getCurrentDate } from '../guards/helpers';
import { ExcelexportService } from 'src/app/core/service/excelexport.service';
import { CommonHttpService } from '../services/common-http.service';
import { ComboInfo } from '../shared/models/models';

@Component({
  selector: 'app-staffperformance',
  templateUrl: './staffperformance.component.html',
  styleUrls: ['./staffperformance.component.scss']
})
export class StaffperformanceComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  staff$: Observable<ComboInfo[]> = of([]);
  staffperformance$: Observable<Root<PaginationResult[]>> = of();
  staffperformances!: StaffPerformance[];
  totalData!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;

  constructor(
    private staffperformanceNewHttpService: StaffperformanceService, 
    private fb: FormBuilder,
    private excelExportService: ExcelexportService,
    private commonHttpService : CommonHttpService
    ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadstaff();
  }

  initializeForm() {
    this.form = this.fb.group({
      staffId: [0],
      startDate: [getCurrentDate()],
      endDate:  [getCurrentDate()],
    });
  }

  loadstaffperformances(page: number): void {
    this.isPaginating = true;
  
    const startDate = this.form.get('startDate')?.value;
    const endDate =this.form.get('endDate')?.value;
    const staffId = this.form.get('staffId')?.value;
  
    const data: StaffPerformanceRequest = {
      hash: GetAuthDetails().hash,
      page: page,
      pageSize: this.pageSize
    };  
    this.staffperformanceNewHttpService.getAllstaffperformances(data, startDate, endDate, staffId).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.staffperformance$ = of(response);
          this.isPaginating = false;
        }
      },
      error: error => {
      }
    });
    
  }

  printData() {
    this.staffperformance$.subscribe((data) => {
      const staffPerformanceData = data.result; 
      this.excelExportService.exportToExcel(staffPerformanceData, 'Staff_Performance');
    });
  }

  tableRefresh() {
    this.loadstaffperformances(this.page);
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadstaffperformances(newPage);
  }


  loadstaff() {
    const query =
      "SELECT id as _Id, name as _Value FROM tblstaffs where status = 0";
    this.staff$ = this.commonHttpService.getComboBoxData(query);
  }

}
