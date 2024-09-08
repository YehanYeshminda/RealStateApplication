import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { Router } from '@angular/router';
import { VenderService } from '../Services/vender.service';
import { VvenderListAll, venderregister } from '../Models/vebderregister';

@Component({
  selector: 'app-venderlist',
  templateUrl: './venderlist.component.html',
  styleUrls: ['./venderlist.component.scss']
})
export class VenderlistComponent implements OnInit {
  venderinfo$: Observable<Root<VvenderListAll>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;

  constructor(
    private venderHttpService: VenderService, 
    private router: Router) { }

  ngOnInit(): void {
    this.loadvender(this.page);
  }

  tableRefresh() {
    this.loadvender(1);
    this.isPaginating = true;
  }

  loadvender(page: number){
		this.venderHttpService.getvender(page, this.pageSize).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.venderinfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }
    })
	}

  editvender(vender: venderregister) {
    this.router.navigateByUrl('/dashboard/vender', { state: vender });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadvender(newPage);
  }

  // deletevender(id: number) {
	// 	confirmDeleteNotification("Are you sure you want to delete this vender").then(response => {
	// 		if (response.isConfirmed) {
	// 			this.venderHttpService.deletevender(id).subscribe({
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
