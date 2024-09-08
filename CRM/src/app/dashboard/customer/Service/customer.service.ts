import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { customer } from '../customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getcustomer(): Observable<Root<customer[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<customer[]>>(this.baseUrl + "customer/getcustomer", auth);
	}

	addcustomer(customer: customer) {
		return this.http.post<Root<customer[]>>(
			this.baseUrl + 'customer/insertcustomer',
			customer
		);
	}

	updatecustomer(customer: customer) {
		return this.http.post<Root<customer[]>>(
			this.baseUrl + 'customer/updatecustomer',
			customer
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'customer/report');
	}
	  
	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'customer/cellreport/' + id);
	}
}
