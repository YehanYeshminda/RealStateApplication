import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { Router } from '@angular/router';
import { PropertyregisterService } from '../Service/propertyregister.service';
import { PropertyRegisterView, VRegListAll, propertyregister } from '../Model/propertyregister';

@Component({
  selector: 'app-propertyregisterlist',
  templateUrl: './propertyregisterlist.component.html',
  styleUrls: ['./propertyregisterlist.component.scss']
})
export class PropertyregisterlistComponent implements OnInit {
  ppropreginfo$: Observable<Root<VRegListAll>> = of();
  totalPages!: number;
  page = 1;
  pageSize = 10;
  isPaginating: boolean = false;

  constructor(
    private propertyHttpService: PropertyregisterService, 
    private router: Router) { }

  ngOnInit(): void {
    this.loadppropreg(this.page);
  }

  tableRefresh() {
    this.loadppropreg(1);
    this.isPaginating = true;
  }

  loadppropreg(page: number){
		this.propertyHttpService.getpropertyregister(page, this.pageSize).subscribe({
      next: response => {
        if (response.isSuccess) {
          this.ppropreginfo$ = of(response);
          this.totalPages = response.result.totalData;
          this.isPaginating = false;
        }
      }
    })
	}

  editppropreg(ppropreg: PropertyRegisterView) {
    this.router.navigateByUrl('/dashboard/ppropreg', { state: ppropreg });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.isPaginating = true;
    this.loadppropreg(newPage);
  }

  // deleteppropreg(id: number) {
	// 	confirmDeleteNotification("Are you sure you want to delete this ppropreg").then(response => {
	// 		if (response.isConfirmed) {
	// 			this.ppropregHttpService.deleteppropreg(id).subscribe({
	// 				next: response => {
	// 					if (response.isSuccess) {
	// 						successNotification(response.message);
	// 						this.loadppropreg(1);
	// 					} else {
	// 						errorNotification(response.message);
	// 					}
	// 				}
	// 			})
	// 		}
	// 	})
	// }
}
