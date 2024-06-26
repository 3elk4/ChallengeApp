import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthenticationService } from "../../../shared/services/authentication/authentication.service";
import { UserRegistrationDto } from "../../../shared/services/authentication/dto/user-registration-dto";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";



@Component({
  selector: 'app-register-user',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterUserComponent implements OnInit {
  form: FormGroup = new FormGroup({ email: new FormControl(''), password: new FormControl('') });
  submitted = false;

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.pattern(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{6,}/)
          ]
        ]
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  public onSubmit() {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    this.authService.registerUser({ email: this.f['email'].value, password: this.f['password'].value }).subscribe({
      next: (_) => this.router.navigateByUrl("/authentication/login"),
      error: (_) => this.toastr.error("Server error")
    });
  }
}
