import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

// utility
import { loggedInUser } from '../helpers/utils';

// types
import { User } from '../models/auth.models';
import { LoginUserRequest, LoginUserResponse } from 'src/app/auth/login/models/loginn';
import { Branch } from './models/branch';
import { environment } from 'src/environments/environment';
import { errorNotification } from 'src/app/pages/dashboards/shared/notifications/notification';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    user: User | null = null;
    baseUrl = environment.apiUrl;

    constructor (private http: HttpClient) {
    }

    /**
     * Returns the current user
     */
    public currentUser(): User | null {
        if (!this.user) {
            this.user = loggedInUser();
        }
        return this.user;
    }

    /**
     * Performs the login auth
     * @param email email of user
     * @param password password of user
     */
    login(data: LoginUserRequest): Observable<any> {

        return this.http.post<any>(this.baseUrl + `auth/Login`, data)
            .pipe(map(user => {
                // login successful if there's a jwt token in the response
                if (user) {
                    //this.user = user;
                    // store user details and jwt in session

                    if (user.isSuccess) {
                        sessionStorage.setItem('currentUser', JSON.stringify(user));
                    } else {
                        return user;
                    }
                }
                return user;
            }));
    }

    /**
     * Performs the signup auth
     * @param name name of user
     * @param email email of user
     * @param password password of user
     */
    signup(name: string, email: string, password: string): Observable<User> {
        return this.http.post<User>(`/api/signup`, { name, email, password })
            .pipe(map(user => user));

    }



    /**
     * Logout the user
     */
    logout(): void {
        // remove user from session storage to log user out
        sessionStorage.removeItem('currentUser');
        this.user = null;
    }

    getAllBranches(): Observable<Branch[]> {
        return this.http.get<Branch[]>(this.baseUrl + "branchs")
      }
}

