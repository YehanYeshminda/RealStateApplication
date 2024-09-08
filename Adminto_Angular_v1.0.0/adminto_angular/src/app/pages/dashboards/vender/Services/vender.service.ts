import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { HtmlResponse } from '../../staff/models/staff';
import { VvenderListAll, venderregister } from '../Models/vebderregister';
import { UniquePK } from '../../shared/UniquePK';

@Injectable({
  providedIn: 'root'
})
export class VenderService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	// getvender(auth: AuthDetails): Observable<Root<venderregister[]>> {
	// 	return this.http.post<Root<venderregister[]>>(
	// 		this.baseUrl + 'supplier/GetAllSuppliers',
	// 		auth
	// 	);
	// }

	
	getvender(page: number = 1, pageSize: number = 2): Observable<Root<VvenderListAll>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<VvenderListAll>>(this.baseUrl + "supplier/GetAllSuppliers", auth, {
			params: {
				page: page,
				pageSize: pageSize
			}
		});
	}
	addvender(vender: venderregister) {
		return this.http.post<Root<venderregister[]>>(
			this.baseUrl + 'supplier/AddNewSupplier',
			vender
		);
	}

	updatevender(vender: venderregister) {
		return this.http.post<Root<venderregister[]>>(
			this.baseUrl + 'supplier/updatesupplier',
			vender
		);
	}

	getUniquePK(): Observable<UniquePK> {
		return this.http.post<UniquePK>(
			this.baseUrl + 'vender/GetUniquePK',
			{}
		);
	}
	
	deletevender(supplierId: number): Observable<any> {
		console.log(supplierId);
		return this.http.delete(`${this.baseUrl}vender/delete/${supplierId}`, {
		});
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'supplier/report');
	}
	  
	cellreport(supplierId: number): Observable<any> {
		return this.http.get(this.baseUrl + 'supplier/cellreport/' + supplierId);
	}
}
