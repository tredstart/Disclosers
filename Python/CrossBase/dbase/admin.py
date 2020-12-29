from django.contrib import admin

# Register your models here.
from django.contrib.auth.models import Group, User

from dbase.models import BaseObject

admin.site.unregister((Group, User))
admin.site.register(
    BaseObject,
)