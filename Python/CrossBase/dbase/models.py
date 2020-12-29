from django.db import models


# Create your models here.
class BaseObject(models.Model):
    db_name = models.CharField(max_length=200, unique=True)
    data = models.JSONField(null=True)

    def __str__(self):
        return self.db_name
