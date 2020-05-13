import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user-service';
import { NavbarFooterService } from '../../_services/nav-service';
import { AdminService } from '../../_services/admin-service';

@Component({
    selector: 'app-admin-users',
    templateUrl: './admin-users.component.html',
    styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit{
    ngOnInit(): void {
        this.nav.hide()
    }
    private users: IUser[]
    private total: number;
    private page: number;
    private limit: number;

    public ableToGoNextPage(): boolean{
        return this.page*this.limit < this.total;
    }
    public ableToGoPreviousPage() : boolean{
        return this.page > 1;
    } 

    constructor(private userService: UserService,
        private adminService: AdminService,
        private nav: NavbarFooterService) {
        this.page = 1;
        this.limit = 100;
        this.getUsers();
    }

    getUsers(): void {
        this.users = this.userService.getUsers(this.page, this.limit)
        this.total = this.userService.getTotal()
    }

    goToPrevious(): void {
        //console.log('Previous Button Clicked!');
        if (this.ableToGoPreviousPage()) {
            this.page--;
            this.getUsers();
        }
    }

    goToNext(): void {
        //console.log('Next Button Clicked!');
        if (this.ableToGoNextPage()) {
            this.page++;
            this.getUsers();
        }
    }

    silence(userName: string): void {
        let days = document.getElementById("silence" + userName).value
        this.adminService.silenceUser(userName, days)
    }


    ban(userName: string): void {
        let days = document.getElementById("ban" + userName).value
        this.adminService.banUser(userName, days)
    }
}
