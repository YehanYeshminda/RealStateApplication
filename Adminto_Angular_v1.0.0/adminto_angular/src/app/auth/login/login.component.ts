import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, first, of } from 'rxjs';

// service
import { AuthenticationService } from 'src/app/core/service/auth.service';

// types
import { User } from 'src/app/core/models/auth.models';
import { LoginUserRequest, LoginUserResponse } from './models/loginn';
import { Branch } from 'src/app/core/service/models/branch';
import { errorNotification } from 'src/app/pages/dashboards/shared/notifications/notification';

@Component({
  selector: 'app-auth-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', Validators.required],
    branchId: [1, [Validators.required]],
  });
  
  formSubmitted: boolean = false;
  error: string = '';
  returnUrl: string = '/';
  loading: boolean = false;
  branches$: Observable<Branch[]> = of([]);

  constructor (
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || this.returnUrl;
    this.getAllBranches();
  }

  getAllBranches() {
    this.branches$ = this.authenticationService.getAllBranches();
  }

  /**
   * convenience getter for easy access to form fields
   */
  get formValues() { return this.loginForm.controls; }

  /**
  * On submit form
  */
  onSubmit(): void {
    this.formSubmitted = true;
    if (this.loginForm.valid) {
      const data: LoginUserRequest = {
        username: this.loginForm.controls['username'].value,
        password: this.loginForm.controls['password'].value,
        branchId: this.loginForm.controls['branchId'].value
      }

      this.loading = true;
      this.authenticationService.login(data)
        .pipe(first())
        .subscribe(
          (data: any) => {
            if (!data.isSuccess) {
              this.loading = false;
              errorNotification(data.message);
            } else {
              this.router.navigate([this.returnUrl]);
            }
          },
          (error: string) => {
            this.error = error;
            
          });
    }
  }


}
