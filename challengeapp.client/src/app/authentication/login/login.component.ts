import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../shared/services/authentication/authentication.service';
import { UserLoginDto } from '../../../shared/services/authentication/dto/user-login-dto';
import { UserLoginResult } from '../../../shared/services/authentication/dto/user-login-result';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, of } from 'rxjs';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login-user',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginUserComponent implements OnInit {
  form: FormGroup = new FormGroup({ email: new FormControl(''), password: new FormControl('') });
  submitted = false;

  errorMessage = '';

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        email: ['', [Validators.required]],
        password: ['',[Validators.required]]
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    this.authService.loginUser({ email: this.f['email'].value, password: this.f['password'].value }).subscribe(
      (res : UserLoginResult) => {
        sessionStorage.setItem('authToken', res.accessToken);
        this.router.navigateByUrl("/");
      },
      err => {
        if (err.status == 401) {
          this.toastr.error("Email or password are incorrect");
        }
        else {
          this.toastr.error("Server error");
        }
      }
    );
  }
}
