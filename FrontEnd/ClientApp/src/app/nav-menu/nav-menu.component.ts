import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../_services/authentification-service';
import { NavbarFooterService } from '../_services/nav-service';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
    currentUser: IUser;

    isAdmin: boolean

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private nav: NavbarFooterService
    ) {
        this.authenticationService.currentUser.subscribe(x => { this.currentUser = x });
        this.authenticationService.currentUser.subscribe(x => { this.isAdmin = x.userRole.id == 1 });
    }



    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}
