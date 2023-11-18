CREATE MIGRATION m1t3bc3vl3xg5blgklfvg2nglimqh3gcoylnuvepgi5kikoujsur6a
    ONTO m1nyaihvekg4re4wdzikksr4xa6onsjzzb324iai7yvmqdnlzlqi7a
{
  DROP FUTURE nonrecursive_access_policies;
  DROP TYPE default::Movie;
  ALTER TYPE default::Person RENAME TO default::Contact;
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY BirthDate: std::datetime {
          SET REQUIRED USING (<std::datetime>{});
      };
  };
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY Email: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY FirstName: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY LastName: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY MarriageStatus: std::bool {
          SET REQUIRED USING (<std::bool>{});
      };
  };
  ALTER TYPE default::Contact {
      CREATE REQUIRED PROPERTY Title: std::str {
          SET REQUIRED USING (<std::str>{});
      };
  };
  ALTER TYPE default::Contact {
      ALTER PROPERTY name {
          RENAME TO Description;
      };
  };
};
