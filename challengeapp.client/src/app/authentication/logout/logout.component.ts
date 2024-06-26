import { Component } from "@angular/core";
import { Router } from "@angular/router";

@Component({
  selector: 'app-logout-user',
  templateUrl: './logout.component.html'
})
export class LogoutUserComponent {
  constructor(private router: Router) {
    sessionStorage.clear();
    this.router.navigateByUrl("/");
  }
}
