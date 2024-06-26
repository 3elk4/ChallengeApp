import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { AuthenticationService } from "../../../shared/services/authentication/authentication.service";
import { UserProfile } from "../../../shared/services/authentication/dto/user-profile";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html'
})

export class UserProfileComponent implements OnInit {
  profile = {} as UserProfile;

  constructor(private authService: AuthenticationService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.authService.account().subscribe(
      (result: UserProfile) => this.profile = result,
      (_) => this.toastr.error("Server error")
    );
  }
}
