import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { environment } from 'src/environments/environment.development';
import { CheckForPermission, CheckForPermissionReponse, LastValue } from './models/common';
import { GetDynamicexpense, SendDynamicexpense } from '../dynamic-expenseaccount/dynamicexpenses';
import { DynamicExpenseTable } from 'src/app/core/models/dynamic';
import { Root } from 'src/app/shared/models/baseResponse';
import { VExpenseAccountData } from '../paymentschedule/models/paymentschedule';

@Injectable({
  providedIn: 'root'
})

export class CommonHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getComboBoxData(query: string): Observable<any> {
    return this.http.get<Observable<any>>(
      this.baseUrl + 'Common/comboData?query=' + query
    );
  }

  getComboBoxstring(query: string): Observable<any> {
		return this.http.get<Observable<any>>(
			this.baseUrl + 'common/comboDataString?query=' + query
		);
	}

  getGetLastValueFromValue(prefix: string, tableName: string, columnName: string): Observable<LastValue> {
    return this.http.get<LastValue>(
      this.baseUrl + `Common/LeadLastValueWithPrefix?prefix=${prefix}&tableName=${tableName}&columnName=${columnName}`
    );
  }

  getBankNameAndIds(): Observable<Root<VExpenseAccountData[]>> {
    const authDto = getAuthDetails();
    return this.http.post<Root<VExpenseAccountData[]>>(this.baseUrl + "PaymentSchedule/ComboVExpenseData", authDto);
  }

  addDynamicexpense(data: SendDynamicexpense): Observable<GetDynamicexpense> {
    return this.http.post<GetDynamicexpense>(this.baseUrl + "Expenses/AddItemAll", data)
  }

  getdynamicexpense(table: string): Observable<DynamicExpenseTable[]> {
    return this.http.get<DynamicExpenseTable[]>(this.baseUrl + 'Expenses/' + table);
  }

  checkForAccess(data: CheckForPermission): Observable<CheckForPermissionReponse> {
    return this.http.post<CheckForPermissionReponse>(this.baseUrl + "up/GetUserPermissionForView", data)
  }
}
