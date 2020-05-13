import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../_services/user-service';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-user',
    templateUrl: './user-detail.component.html',
    styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit{
    ngOnInit(): void {
        this.nav.show()
    }
    public user: IUser

    constructor(private service: UserService,
        route: ActivatedRoute,
        private nav: NavbarFooterService) {
        this.user = service.getUser(route.snapshot.params['id'])
    }
}
