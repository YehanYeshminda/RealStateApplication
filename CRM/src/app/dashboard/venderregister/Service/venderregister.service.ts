import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { UniquePK } from 'src/app/core/models/UniquePK';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { venderregister } from '../venderregister';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';

@Injectable({
  providedIn: 'root'
})
export class VenderService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getvender(auth: AuthDetails): Observable<Root<venderregister[]>> {
		return this.http.post<Root<venderregister[]>>(
			this.baseUrl + 'supplier/GetAllSuppliers',
			auth
		);
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
