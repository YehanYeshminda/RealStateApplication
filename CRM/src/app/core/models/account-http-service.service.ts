import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Injectable({
	providedIn: 'root',
})
export class AccountHttpServiceService {
	baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) {}

	login(data: any) {
		return this.http.post(this.baseUrl + 'account/login', data);
	}
}
