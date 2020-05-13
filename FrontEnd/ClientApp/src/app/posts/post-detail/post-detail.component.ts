import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PostService } from '../../_services/post-service';
import { CommentService } from '../../_services/comment-service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { formatDate } from '@angular/common';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-post-detail',
    templateUrl: './post-detail.component.html',
    styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {
    public post: IPost
    private url: string
    private user: IUser

    public isUser(): boolean {
        return JSON.parse(localStorage.getItem('currentUser')) != undefined
    }

    constructor(private commentService: CommentService,
        postService: PostService,
        private formBuilder: FormBuilder,
        route: ActivatedRoute,
        private router: Router,
        private nav: NavbarFooterService
    ) {
        this.url = '/posts/'+ route.snapshot.params['id']
        this.post = postService.getPost(route.snapshot.params['id'])

        this.post.comments.sort((a, b) => a.dateCreated > b.dateCreated ? 1 : -1)

        this.user = JSON.parse(localStorage.getItem('currentUser'))
    }

    commForm: FormGroup;
    loading = false;
    submitted = false;
    error = '';

    ngOnInit() {
        this.commForm = this.formBuilder.group({
            message: ['', Validators.required]
        });
        this.nav.show()
    }

    get f() { return this.commForm.controls; }

    onSubmit() {
        this.submitted = true;

        if (this.commForm.invalid) {
            return;
        }

        this.loading = true;

        let date = formatDate(Date.now(), "MM.dd.yyyy HH:mm", 'en-US')

        this.commentService.createComment(
            this.f.message.value,
            this.user.id,
            this.post.id,
            date
        )

        this.loading = false
    }

    goLogIn(): void {
        this.router.navigate(['/login'], { queryParams: { returnUrl: this.url } })
    }
}
