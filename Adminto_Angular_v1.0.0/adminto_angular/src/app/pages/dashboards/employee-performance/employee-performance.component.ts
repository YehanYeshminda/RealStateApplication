import { Component, OnInit } from '@angular/core';
import { EmployeePerformanceHttpService } from './services/employee-performance-http.service';
import { Observable, of, retry, timeout } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { EmployeePerformance } from './models/performance';
import { errorNotification } from '../shared/notifications/notification';

@Component({
  selector: 'app-employee-performance',
  templateUrl: './employee-performance.component.html',
  styleUrls: ['./employee-performance.component.scss']
})
export class EmployeePerformanceComponent implements OnInit {
  employeePerformance$: Observable<Root<EmployeePerformance[]>> = of();
  isGeneratingReport = false;

  constructor(private employeePerformanceHttpService: EmployeePerformanceHttpService) { }

  ngOnInit(): void {
    this.employeePerformance$ = this.employeePerformanceHttpService.getAllEmployeePerformance().pipe(
      timeout(8000),
      retry(3)
    );
  }

  loadToday() {
    this.employeePerformance$ = this.employeePerformanceHttpService.getAllEmployeePerformance().pipe(
      timeout(8000),
      retry(3)
    );
  }

  loadWeekly() {
    this.employeePerformance$ = this.employeePerformanceHttpService.getAllEmployeePerformanceWeekly().pipe(
      timeout(8000),
      retry(3)
    );
  }

  loadMonthly() {
    this.employeePerformance$ = this.employeePerformanceHttpService.getAllEmployeePerformanceMonthly().pipe(
      timeout(8000),
      retry(3)
    );
  }

  generateReportToday() {
    this.isGeneratingReport = true;
    this.employeePerformanceHttpService.generateEmployeePerformanceReportToday().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.isGeneratingReport = false;
          const printWindow = window.open('', '');
          if (printWindow) {
            printWindow.document.open();
            printWindow.document.write(response.result);
            printWindow.document.close();
              printWindow.onload = function () {
                printWindow.print();
                printWindow.close();
            };
          }
        } else {
          errorNotification("Error Occured while generating report")
        }
      }
    });
  }

  generateReportWeekly() {
    this.isGeneratingReport = true;
    this.employeePerformanceHttpService.generateEmployeePerformanceReportWeekly().subscribe({
      next: response => {
        if (response.isSuccess) {
          this.isGeneratingReport = false;
          const printWindow = window.open('', '');
          if (printWindow) {
            printWindow.document.open();
            printWindow.document.write(response.result);
            printWindow.document.close();
              printWindow.onload = function () {
                printWindow.print();
                printWindow.close();
            };
          }
        } else {
          errorNotification("Error Occured while generating report")
        }
      }
    });
  }
}
