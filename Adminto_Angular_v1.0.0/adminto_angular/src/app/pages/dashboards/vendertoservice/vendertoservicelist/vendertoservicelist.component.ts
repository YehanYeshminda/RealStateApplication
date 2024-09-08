import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { Router } from '@angular/router';
import { VendertoserviceService } from '../Services/vendertoservice.service';
import { VVTSListAll, VtsView, vts } from '../Modals/vts';

@Component({
  selector: 'app-vendertoservicelist',
  templateUrl: './vendertoservicelist.component.html',
  styleUrls: ['./vendertoservicelist.component.scss']
})
export class VendertoservicelistComponent implements OnInit {
  vtsinfo$: Observable<Root<VVTSListAll>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;

  constructor(
    private venderHttpServiceservices: VendertoserviceService, 
    private router: Router) { }

  ngOnInit(): void {
    this.loadvender(this.page);
  }

  tableRefresh() {
    this.loadvender(1);
    this.isPaginating = true;
  }

  loadvender(page: number){
		this.venderHttpServiceservices.getVTS(page, this.pageSize).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.vtsinfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }
    })
	}

  editvender(vender: VtsView) {
    this.router.navigateByUrl('/dashboard/vts', { state: vender });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadvender(newPage);
  }

  // deletevender(id: number) {
	// 	confirmDeleteNotification("Are you sure you want to delete this vender").then(response => {
	// 		if (response.isConfirmed) {
	// 			this.venderHttpServiceservices.deletevender(id).subscribe({
	// 				next: response => {
	// 					if (response.isSuccess) {
	// 						successNotification(response.message);
	// 						this.loadvender(1);
	// 					} else {
	// 						errorNotification(response.message);
	// 					}
	// 				}
	// 			})
	// 		}
	// 	})
	// }
}
