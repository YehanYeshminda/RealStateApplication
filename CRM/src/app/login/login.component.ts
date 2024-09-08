import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Branch } from './models/branch';
import { Company } from './models/company';
import { LoginHttpService } from './services/login-http.service';
import { errorNotification } from '../core/models/notification';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

// declare var particlesJS: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, AfterViewInit {
  companies$: Observable<Company> = of();
  loginForm: FormGroup = new FormGroup({});
  branches$: Observable<Branch[]> = of([]);
  logginIn = false;

  constructor(private loginHttpService: LoginHttpService, private fb: FormBuilder, private router: Router, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getCompanyInformation();
    this.getAllBranches();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      branchId: [1, [Validators.required]],
      password: ['', [Validators.required]],
    })
  }

  ngAfterViewInit(): void {
    // this.initializeParticles();
  }

  getCompanyInformation() {
    this.companies$ = this.loginHttpService.getAllCompanies();
  }

  getAllBranches() {
    this.branches$ = this.loginHttpService.getAllBranches();
  }

  login() {
    this.logginIn = true;
    this.spinner.show();

    this.loginHttpService.loginUser(this.loginForm.value).subscribe({
      next: response => {
        if (response.isSuccess) {
          sessionStorage.setItem('user', JSON.stringify(response.result));

          this.router.navigateByUrl("/dashboard/user-home");
          
          this.logginIn = false;
          this.spinner.hide();
        } else {
          errorNotification(response.message);
          this.logginIn = false;
          this.spinner.hide();
        }
      }
    })
  }
}
