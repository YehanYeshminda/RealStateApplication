import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { VExpense, expense, expenseview } from '../expense';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getexpense(): Observable<Root<expenseview[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<expenseview[]>>(this.baseUrl + "Expenses/getexpense", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "Expenses/GetStaffNameIdAll", auth)
	}

	addexpense(expense: expense) {
		return this.http.post<Root<expense[]>>(
			this.baseUrl + 'Expenses/insertexpense',
			expense
		);
	}

	updateexpense(expense: expense) {
		return this.http.post<Root<expense[]>>(
			this.baseUrl + 'Expenses/updateexpense',
			expense
		);
	}

	getMaincatById( id: number) {
		return this.http.post<VExpense>(
			this.baseUrl + 'Expenses/viewVExpenseById?id=' + id,{}
		);
	}

	// getSubcatById( id: number) {
	// 	return this.http.post<VExpense>(
	// 		this.baseUrl + 'Expense/viewVExpenseById?id=' + id,{}
	// 	);
	// }

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'Expenses/report');
	}
	  
	cellreport(id: string): Observable<any> {
		return this.http.get(this.baseUrl + 'Expenses/cellreport/' + id);
	}
}
